# 🔗 Quick Integration Guide

## Đảm Bảo App & AdminWeb Liên Kết

### ✅ TL;DR - Chạy Ngay

```powershell
# 1. Start AdminWeb
cd DoAnCSharp.AdminWeb
.\run-admin.ps1

# 2. Test integration
cd ..
.\test-integration.ps1

# 3. Run MAUI App
# Open in Visual Studio → F5
# API sẽ tự động connect!
```

---

## 🎯 Kiến Trúc Liên Kết

```
📱 MAUI App  →  HTTP/HTTPS  →  💻 AdminWeb  →  🗄️ SQLite Database
              (ApiService)         (Controllers)
```

### Đã Implement:
- ✅ **API-First Architecture** - App gọi AdminWeb qua REST API
- ✅ **Smart URL Configuration** - Tự động detect môi trường (emulator/physical/production)
- ✅ **QR Deep Linking** - Scan QR → Open app hoặc web
- ✅ **Device Tracking** - Giới hạn 5 scans/day, analytics
- ✅ **CORS Enabled** - Cho phép cross-origin requests

---

## 📝 Configuration

### Development (Android Emulator):
```csharp
// Tự động dùng: http://10.0.2.2:5000/api
ApiConfiguration.CurrentEnvironment = AppEnvironment.Development;
```

### Physical Device (Same WiFi):
```csharp
// Tìm IP máy: ipconfig → 192.168.1.100
var apiService = new ApiService(
    ApiConfiguration.GetPhysicalDeviceUrl("192.168.1.100")
);
```

### Production:
```csharp
// Before building release APK/IPA
ApiConfiguration.CurrentEnvironment = AppEnvironment.Production;
// API URL: https://api.vinhkhanhtour.com/api
```

---

## 🧪 Testing

### Test 1: Run Integration Test
```powershell
.\test-integration.ps1
```
Kiểm tra:
- ✅ AdminWeb đang chạy
- ✅ API endpoints hoạt động
- ✅ QR code system OK
- ✅ Database có dữ liệu

### Test 2: App API Connection
```csharp
// Trong App.xaml.cs hoặc MainPage
protected override async void OnAppearing()
{
    // Test connection
    bool connected = await ApiConfiguration.TestConnectionAsync();
    
    if (connected)
    {
        var api = new ApiService();
        var pois = await api.GetPOIsAsync();
        Debug.WriteLine($"✅ Got {pois.Count} POIs from API");
    }
}
```

### Test 3: QR Scan Flow
1. AdminWeb → Tab "🏪 Quán Ăn" → Thêm quán (auto tạo QR)
2. Click "👁️ Xem QR" → Copy QR code (POI_XXXXXXXXXX)
3. MAUI App → Scan QR hoặc input manual
4. App gọi: `GET /api/pois/qr/POI_XXX`
5. Hiển thị thông tin quán

---

## 📊 API Endpoints Được App Dùng

| Endpoint | App Usage |
|----------|-----------|
| `GET /api/pois` | MapPage - Load tất cả quán |
| `GET /api/pois/{id}` | POI detail |
| `GET /api/pois/qr/{code}` | ScanQRPage - Tìm quán theo QR |
| `GET /api/users` | ProfilePage - User list |
| `POST /api/users` | Đăng ký user mới |
| `PUT /api/users/{id}` | Cập nhật profile |
| `GET /qr-scan?code=XXX` | Deep link landing |

---

## 🚀 Deployment Checklist

### Before Production:
- [ ] Update `ApiConfiguration.CurrentEnvironment = AppEnvironment.Production`
- [ ] Set production API URL
- [ ] Test API connectivity
- [ ] Configure deep linking (Android/iOS)
- [ ] Build release APK/IPA
- [ ] Deploy AdminWeb to Azure/VPS
- [ ] Setup HTTPS/SSL
- [ ] Test end-to-end flow

---

## 📖 Full Documentation

- **[APP_ADMINWEB_INTEGRATION.md](docs/APP_ADMINWEB_INTEGRATION.md)** - Chi tiết đầy đủ
- **[QR_CODE_IMPLEMENTATION.md](docs/QR_CODE_IMPLEMENTATION.md)** - QR code system
- **[FIXES_SUMMARY.md](docs/FIXES_SUMMARY.md)** - Tổng hợp fixes

---

## 🆘 Troubleshooting

### ❌ App không connect được AdminWeb?

**Emulator:**
```csharp
// Đảm bảo dùng 10.0.2.2 (NOT localhost)
ApiConfiguration.CurrentEnvironment = AppEnvironment.Development;
```

**Physical Device:**
```powershell
# 1. Tìm IP máy
ipconfig
# → 192.168.1.100

# 2. Chạy AdminWeb với IP binding
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run --urls "http://0.0.0.0:5000"

# 3. Update app
var api = new ApiService("http://192.168.1.100:5000/api");
```

**Check:**
- ✅ Cùng WiFi
- ✅ Firewall allow port 5000
- ✅ AdminWeb đang chạy

### ❌ QR Scan không hoạt động?

```csharp
// Debug QR lookup
var api = new ApiService();
var poi = await api.GetPOIByQRCodeAsync("POI_XXXXXXXXXX");

if (poi == null)
{
    Debug.WriteLine("❌ QR Code not found in database");
    // → Check AdminWeb: POI có QRCode field?
}
```

---

## ✨ Summary

**Hệ thống đã được thiết kế để tự động liên kết:**

1. **App** → Dùng `ApiConfiguration` → Tự động chọn URL đúng
2. **AdminWeb** → CORS enabled → Accept requests từ app
3. **Database** → Shared qua API (không truy cập trực tiếp)
4. **QR Codes** → Tạo tự động, deep linking hoạt động
5. **Testing** → Run `test-integration.ps1` để verify

**Chỉ cần:**
- ✅ Run AdminWeb
- ✅ Run App
- ✅ Everything works! 🎉

**Lưu ý Production:**
- Đổi `CurrentEnvironment` trước khi build release
- Deploy AdminWeb lên server với HTTPS
- Test API từ nhiều devices

---

**📱 Happy Coding!**
