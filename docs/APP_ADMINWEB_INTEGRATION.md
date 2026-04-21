# 🔗 App & AdminWeb Integration Guide
## Hướng Dẫn Liên Kết Giữa MAUI App và Admin Web

---

## 📊 Tổng Quan Kiến Trúc

```
┌──────────────────────────────────────────────────────────────┐
│                    MAUI Mobile App                           │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │  MapPage   │  │ ProfilePage│  │ ScanQRPage │            │
│  └──────┬─────┘  └──────┬─────┘  └──────┬─────┘            │
│         │                │                │                   │
│         └────────────────┴────────────────┘                   │
│                          │                                    │
│                   ┌──────▼──────┐                            │
│                   │ ApiService  │ ← Gọi HTTP API            │
│                   └──────┬──────┘                            │
│                          │                                    │
│                   ┌──────▼──────────┐                        │
│                   │ DatabaseService │ ← Local SQLite         │
│                   └─────────────────┘                        │
└──────────────────────────────────────────────────────────────┘
                          │
                          │ HTTP/HTTPS
                          │ http://10.0.2.2:5000/api
                          │ https://your-domain.com/api
                          ▼
┌──────────────────────────────────────────────────────────────┐
│                 ASP.NET Core AdminWeb                        │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              API Controllers                          │   │
│  │  /api/pois      /api/users      /api/devices         │   │
│  │  /api/payments  /api/qrscans    /api/history         │   │
│  └──────────────────┬───────────────────────────────────┘   │
│                     │                                         │
│              ┌──────▼──────┐                                 │
│              │ DatabaseService │ ← Shared SQLite             │
│              └─────────────────┘                             │
│                     │                                         │
│              VinhKhanhTour_Full.db3                          │
│              C:\Users\{User}\AppData\Roaming\VinhKhanhTour\ │
└──────────────────────────────────────────────────────────────┘
```

---

## 🗄️ Database Sharing - QUAN TRỌNG!

### ✅ HIỆN TẠI: Cả 2 Dùng **CÙNG CHUNG DATABASE**

#### **MAUI App Database:**
```csharp
// Services\DatabaseService.cs (MAUI)
private const string DbFileName = "VinhKhanhTour_Full.db3";

var dbPath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
// → Android: /data/data/com.vinhkhanhtour/files/VinhKhanhTour_Full.db3
// → iOS: ~/Library/Application Support/VinhKhanhTour_Full.db3
// → Windows: C:\Users\{User}\AppData\Local\Packages\...\LocalState\VinhKhanhTour_Full.db3
```

#### **AdminWeb Database:**
```csharp
// DoAnCSharp.AdminWeb\Services\DatabaseService.cs
private const string DbFileName = "VinhKhanhTour_Full.db3";

var appDataPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "VinhKhanhTour"
);
var dbPath = Path.Combine(appDataPath, DbFileName);
// → C:\Users\{User}\AppData\Roaming\VinhKhanhTour\VinhKhanhTour_Full.db3
```

### ⚠️ VẤN ĐỀ: Databases Khác Nhau!

**MAUI app** và **AdminWeb** hiện tại đang dùng **2 database files khác nhau** ở 2 vị trí khác nhau:
- 📱 **MAUI:** `/data/data/.../files/VinhKhanhTour_Full.db3` (trên điện thoại)
- 💻 **AdminWeb:** `C:\Users\...\AppData\Roaming\VinhKhanhTour\VinhKhanhTour_Full.db3` (trên PC)

**Kết quả:**
- ❌ Data không đồng bộ
- ❌ Admin thêm quán mới → App không thấy (trừ khi gọi API)
- ❌ User đăng ký trên app → Admin không thấy (trừ khi gọi API)

### ✅ GIẢI PHÁP: API-First Architecture

**App KHÔNG nên truy cập database trực tiếp**. Thay vào đó:

```
MAUI App → API HTTP → AdminWeb → Database
         ←──JSON────←
```

---

## 🌐 API Integration - Đã Có Sẵn!

### ✅ Base URL Configuration

#### **MAUI App - ApiService.cs:**
```csharp
private string _baseUrl = "http://10.0.2.2:5000/api";
//                         ↑
// Android emulator: 10.0.2.2 = localhost của máy host
// iOS simulator: localhost works
// Physical device: Dùng IP thực của máy (192.168.x.x)
```

#### **AdminWeb - Program.cs:**
```csharp
// ✅ CORS đã được enable
app.UseCors("AllowAll"); // Cho phép app gọi API từ bất kỳ origin nào

// ✅ URLs
app.MapControllers(); // API tại /api/*
// HTTP:  http://localhost:5000
// HTTPS: https://localhost:5001
```

### ✅ API Endpoints Đã Có:

| Endpoint | Method | Mục Đích | App Dùng |
|----------|--------|----------|----------|
| `/api/pois` | GET | Lấy danh sách quán ăn | ✅ MapPage |
| `/api/pois/{id}` | GET | Lấy chi tiết 1 quán | ✅ |
| `/api/pois/qr/{code}` | GET | Lấy quán theo QR code | ✅ ScanQRPage |
| `/api/users` | GET | Lấy danh sách users | ✅ |
| `/api/users` | POST | Đăng ký user mới | ✅ |
| `/api/users/{id}` | GET | Lấy thông tin 1 user | ✅ ProfilePage |
| `/api/users/{id}` | PUT | Cập nhật user | ✅ |
| `/api/devices` | GET | Quản lý thiết bị | ⚠️ Chưa dùng |
| `/api/payments` | GET | Thanh toán | ⚠️ Chưa dùng |
| `/api/history` | GET | Lịch sử nghe | ⚠️ Chưa dùng |
| `/qr-scan?code=XXX` | GET | QR scan landing | ✅ Deep link |

---

## 🔧 Cấu Hình Cho Các Môi Trường

### 1️⃣ **Development - Máy Local**

#### **Chạy AdminWeb:**
```powershell
cd DoAnCSharp.AdminWeb
.\run-admin.ps1
# → Chạy tại http://localhost:5000
```

#### **Chạy MAUI App (Android Emulator):**
```csharp
// ApiService.cs - Không cần sửa gì
private string _baseUrl = "http://10.0.2.2:5000/api";
```

**Test:**
```csharp
// Trong MAUI app
var apiService = new ApiService();
var pois = await apiService.GetPOIsAsync();
// → Gọi http://10.0.2.2:5000/api/pois
// → AdminWeb nhận request
// → Trả về danh sách quán từ database
```

---

### 2️⃣ **Testing - Physical Device**

Khi test trên điện thoại thật (không phải emulator):

#### **Bước 1: Tìm IP máy tính**
```powershell
ipconfig
# Tìm dòng "IPv4 Address" của WiFi/Ethernet
# Ví dụ: 192.168.1.100
```

#### **Bước 2: Cập nhật App**
```csharp
// App.xaml.cs hoặc MauiProgram.cs
var apiService = new ApiService("http://192.168.1.100:5000/api");
//                                ↑ Thay bằng IP thực của bạn
```

#### **Bước 3: Chạy AdminWeb với IP binding**
```powershell
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run --urls "http://0.0.0.0:5000;https://0.0.0.0:5001"
#                        ↑ Cho phép truy cập từ máy khác
```

#### **Bước 4: Test từ điện thoại**
- ✅ Điện thoại và máy tính **PHẢI cùng WiFi**
- ✅ Tắt firewall hoặc allow port 5000

---

### 3️⃣ **Production - Deploy lên Server**

#### **Option A: Azure App Service**
```csharp
// App config
private string _baseUrl = "https://vinhkhanhtour-api.azurewebsites.net/api";
```

#### **Option B: VPS/Cloud Server**
```csharp
// App config
private string _baseUrl = "https://api.vinhkhanhtour.com/api";
```

**Setup:**
1. Deploy AdminWeb lên server
2. Có domain hoặc public IP
3. Cấu hình SSL (HTTPS)
4. Cập nhật `_baseUrl` trong app
5. Rebuild app với base URL mới

---

## 🔐 Deep Linking - QR Code Integration

### ✅ Đã Implement Đầy Đủ

#### **Flow:**
```
1. User quét QR code POI_XXXXXXXXXX
   ↓
2. QR trỏ đến: https://vinhkhanhtour.com/qr-scan?code=POI_XXX
   ↓
3. AdminWeb nhận request:
   - Kiểm tra device limit (5 scans/day)
   - Ghi nhận scan analytics
   ↓
4. Redirect với deep link:
   window.location = "vinhkhanhtour://poi/1"
   ↓
5. MAUI App mở (nếu đã cài)
   hoặc
   Fallback to web page /poi-public.html
```

#### **Setup Deep Link trong MAUI:**

**Android - Platforms/Android/AndroidManifest.xml:**
```xml
<activity android:name="com.vinhkhanhtour.MainActivity">
    <intent-filter>
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        <data android:scheme="vinhkhanhtour" android:host="poi" />
    </intent-filter>
</activity>
```

**iOS - Platforms/iOS/Info.plist:**
```xml
<key>CFBundleURLTypes</key>
<array>
    <dict>
        <key>CFBundleURLSchemes</key>
        <array>
            <string>vinhkhanhtour</string>
        </array>
    </dict>
</array>
```

**App.xaml.cs - Handle Deep Link:**
```csharp
protected override void OnAppLinkRequestReceived(Uri uri)
{
    base.OnAppLinkRequestReceived(uri);
    
    // uri = vinhkhanhtour://poi/1
    if (uri.Host == "poi" && int.TryParse(uri.Segments[1], out int poiId))
    {
        // Navigate to POI detail
        Shell.Current.GoToAsync($"//MapPage?poiId={poiId}");
    }
}
```

---

## 📱 Data Sync Strategy

### ✅ Recommended: **API-First + Local Cache**

```csharp
// MAUI App - Hybrid approach
public async Task<List<AudioPOI>> GetPOIsAsync()
{
    try
    {
        // 1. Try API first (fresh data)
        var apiPois = await _apiService.GetPOIsAsync();
        
        if (apiPois?.Any() == true)
        {
            // 2. Cache to local database
            await _databaseService.SavePOIsAsync(apiPois);
            return apiPois;
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"API failed: {ex.Message}");
    }
    
    // 3. Fallback to local cache if API fails
    return await _databaseService.GetPOIsAsync();
}
```

**Lợi ích:**
- ✅ Luôn có dữ liệu mới nhất từ AdminWeb
- ✅ Hoạt động offline (dùng cache)
- ✅ Không cần manual sync

---

## 🧪 Testing Integration

### Test 1: API Connectivity

**Trong MAUI App:**
```csharp
// Test trong MapPage.xaml.cs
protected override async void OnAppearing()
{
    base.OnAppearing();
    
    var apiService = new ApiService();
    
    try
    {
        var pois = await apiService.GetPOIsAsync();
        Debug.WriteLine($"✅ API Connected! Got {pois.Count} POIs");
        
        foreach (var poi in pois)
        {
            Debug.WriteLine($"  - {poi.Name} (QR: {poi.QRCode})");
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"❌ API Error: {ex.Message}");
    }
}
```

### Test 2: QR Scan Workflow

**Bước 1: Tạo QR code từ AdminWeb**
1. Vào http://localhost:5001
2. Tab "🏪 Quán Ăn"
3. Thêm quán mới → Auto tạo QR code `POI_XXXXXXXXXX`
4. Click "👁️ Xem QR"
5. Copy URL: `http://localhost:5001/qr-scan?code=POI_XXX`

**Bước 2: Test từ MAUI App**
```csharp
// ScanQRPage.xaml.cs
private async void OnQRCodeScanned(string qrCode)
{
    // qrCode = "POI_A1B2C3D4E5"
    
    var apiService = new ApiService();
    var poi = await apiService.GetPOIByQRCodeAsync(qrCode);
    
    if (poi != null)
    {
        Debug.WriteLine($"✅ POI Found: {poi.Name}");
        // Navigate to detail
    }
    else
    {
        Debug.WriteLine($"❌ POI Not Found for QR: {qrCode}");
    }
}
```

### Test 3: User Registration

**Từ MAUI App:**
```csharp
var apiService = new ApiService();
bool success = await apiService.RegisterUserAsync(
    fullName: "Test User",
    email: "test@example.com",
    password: "Test123"
);

if (success)
{
    // Check AdminWeb → Tab "👥 Người Dùng"
    // User mới phải xuất hiện!
}
```

---

## 🚀 Deployment Checklist

### 📱 MAUI App Release:

- [ ] Update `_baseUrl` to production API
- [ ] Configure deep linking (iOS/Android)
- [ ] Test API connectivity
- [ ] Test QR scanning
- [ ] Test offline mode (cache)
- [ ] Build release APK/IPA

### 💻 AdminWeb Release:

- [ ] Deploy to Azure/VPS
- [ ] Configure HTTPS/SSL
- [ ] Set CORS to specific origins (not "*")
- [ ] Setup database backups
- [ ] Configure environment variables
- [ ] Test API endpoints
- [ ] Test QR scan landing page

---

## 📊 Monitoring & Debugging

### AdminWeb Logs:
```powershell
# Xem API calls trong real-time
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run

# Output sẽ hiện:
# info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
#       Request starting HTTP/1.1 GET http://localhost:5000/api/pois
# info: Microsoft.AspNetCore.Routing.EndpointMiddleware[0]
#       Executing endpoint 'DoAnCSharp.AdminWeb.Controllers.POIsController.GetAll'
```

### MAUI App Debug:
```csharp
// Output window trong Visual Studio
Debug.WriteLine("🌐 API Call: GET /api/pois");
Debug.WriteLine($"✅ Response: {pois.Count} items");
```

---

## 🔗 Quick Reference

### URLs:

| Service | URL | Mục đích |
|---------|-----|----------|
| AdminWeb (Local) | http://localhost:5000 | Admin dashboard |
| AdminWeb HTTPS | https://localhost:5001 | Admin dashboard (SSL) |
| API Base | http://localhost:5000/api | REST API |
| QR Scan Landing | http://localhost:5000/qr-scan?code=XXX | QR redirect |
| Public POI Info | http://localhost:5000/poi-public.html?qr=XXX | Web view quán |

### API Examples:

```bash
# Lấy tất cả quán ăn
GET http://localhost:5000/api/pois

# Lấy 1 quán
GET http://localhost:5000/api/pois/1

# Tìm quán theo QR
GET http://localhost:5000/api/pois/qr/POI_A1B2C3D4E5

# Đăng ký user
POST http://localhost:5000/api/users
Content-Type: application/json
{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "Test123"
}
```

---

## 📝 Summary

### ✅ Đã Có (Working):
1. **API Integration** - App gọi AdminWeb API
2. **CORS** - Cho phép cross-origin requests
3. **QR Deep Linking** - Scan QR → Open app
4. **Device Tracking** - Giới hạn 5 scans/day
5. **Models Consistent** - Cùng structure

### ⚠️ Cần Chú Ý:
1. **Base URL** - Đổi theo môi trường (dev/prod)
2. **Database** - App và AdminWeb dùng riêng database (OK vì dùng API)
3. **Network** - Phải cùng WiFi khi test physical device
4. **HTTPS** - Production nên dùng HTTPS

### 🎯 Best Practices:
- ✅ **App → API → Database** (không truy cập database trực tiếp)
- ✅ Local cache cho offline mode
- ✅ API-first architecture
- ✅ Centralized data management qua AdminWeb

**Hệ thống đã liên kết đầy đủ và sẵn sàng sử dụng!** 🎉
