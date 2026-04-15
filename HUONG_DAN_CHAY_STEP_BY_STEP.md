═══════════════════════════════════════════════════════════════════
        🔧 HƯỚNG DẪN CHẠY MAUI APP + WEB ADMIN KẾT NỐI
═══════════════════════════════════════════════════════════════════

## 📍 BƯỚC 1: Chạy Web Admin

**Cách 1 (Nhanh nhất):**
   1. Windows Explorer
   2. Đi tới: C:\Users\LENOVO\source\repos\do_an_C_sharp\
   3. Double-click: run-admin.bat

**Cách 2 (Command Line):**
   ```powershell
   cd C:\Users\LENOVO\source\repos\do_an_C_sharp
   .\run-admin.bat
   ```

**Cách 3 (Terminal trong VS Code/Visual Studio):**
   ```powershell
   cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
   dotnet run
   ```

✅ Khi thấy:
   ```
   Now listening on: http://localhost:5000
   Application started. Press Ctrl+C to shut down.
   ```

👉 Mở browser: http://localhost:5000
   → Thấy dashboard admin là OK!

═══════════════════════════════════════════════════════════════════

## 📍 BƯỚC 2: Chạy MAUI App (Terminal/Shell mới)

```powershell
# Từ project root
cd C:\Users\LENOVO\source\repos\do_an_C_sharp

# Chạy MAUI app
dotnet run -f net8.0-android
```

⏳ Chờ app khởi động (có thể mất 30-60 giây lần đầu)

✅ Khi app mở:
   - Thấy trang đăng nhập → Đăng nhập bình thường
   - MAUI sẽ tự động gọi API từ Web Admin
   - Nếu Web Admin down → Auto fallback sang local DB

═══════════════════════════════════════════════════════════════════

## 🎬 DEMO - Kiểm tra kết nối

### Phương án A: Test thêm quán từ Web Admin

1. Web Admin (http://localhost:5000)
   - Click tab [Quán Ăn]
   - Click [+ Thêm quán]
   - Nhập dữ liệu:
     * Tên: "Ốc Test"
     * Địa chỉ: "123 Đường Test"
     * Mô tả: "Test kết nối"
     * Tọa độ: 10.7595, 106.7045
     * Ưu tiên: 1
   - Click [Lưu]

2. MAUI App
   - Reload trang Home (kéo xuống hoặc navigate)
   - Kiểm tra danh sách quán
   - Nếu thấy "Ốc Test" → **KẾT NỐI THÀNH CÔNG! ✓**

═══════════════════════════════════════════════════════════════════

## 🔄 LỰA CHỌN: Single API URL cho cả 2

Nếu bạn muốn MAUI **luôn** gọi Web Admin (không fallback):

**Sửa trong MauiProgram.cs:**
```csharp
// Thay vì:
builder.Services.AddSingleton<ApiService>();

// Thành:
builder.Services.AddSingleton(_ => new ApiService("http://localhost:5000/api"));
```

Hoặc **Dynamic URL** (nhập từ Settings):
```csharp
var apiUrl = Microsoft.Maui.Storage.Preferences.Default.Get("ApiUrl", "http://localhost:5000/api");
builder.Services.AddSingleton(_ => new ApiService(apiUrl));
```

═══════════════════════════════════════════════════════════════════

## ⚙️ CHẠY TRÊN CHỨNG CHỈ KHÔNG AN TOÀN (nếu cần HTTPS)

Nếu Web Admin chạy HTTPS:

```csharp
// Trong ApiService.cs
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
_httpClient = new HttpClient(handler);
```

═══════════════════════════════════════════════════════════════════

## 📡 CHẠY TRÊN LAN (Network khác)

Nếu muốn MAUI chạy trên device/emulator khác (IP khác):

**Bước 1: Start Web Admin trên IP thực tế**
```powershell
# Lấy IP local của bạn
ipconfig

# Giả sử IP là 192.168.1.100
dotnet run --urls "http://192.168.1.100:5000"
```

**Bước 2: Update ApiService URL**
```csharp
var apiService = new ApiService("http://192.168.1.100:5000/api");
```

Hoặc sửa trong appsettings:
```json
{
  "ApiBaseUrl": "http://192.168.1.100:5000/api"
}
```

═══════════════════════════════════════════════════════════════════

## 📝 LOG KIỂM TRA KẾT NỐI

Khi MAUI app chạy, xem Debug Output:

✅ Nếu thấy:
   ```
   ✅ Web Admin API khả dụng - Lấy dữ liệu từ API
   ```
   → Kết nối thành công!

⚠️ Nếu thấy:
   ```
   ⚠️ Web Admin API không khả dụng - Lấy từ local database
   ```
   → Web Admin down, dùng local DB (fallback)

═══════════════════════════════════════════════════════════════════

## 🎯 TÓNG TẮT

┌─────────────────────────────────────┐
│ Terminal 1: Web Admin               │
│ > run-admin.bat                     │
│ http://localhost:5000               │
└─────────────────────────────────────┘
            ↓↑
        API Calls
            ↓↑
┌─────────────────────────────────────┐
│ Terminal 2: MAUI App                │
│ > dotnet run -f net8.0-android      │
│ → Gọi Web API automatically         │
└─────────────────────────────────────┘

✓ 2 bên chạy → Sync real-time
✓ Web Admin down → MAUI dùng local DB
✓ Demo thêm/sửa/xóa từ 1 bên → Hiển thị ở bên kia

═══════════════════════════════════════════════════════════════════

❓ LỖI THƯỜNG GẶP & GIẢI PHÁP:

1️⃣ MAUI không load dữ liệu
   → Check Web Admin có chạy? (http://localhost:5000)
   → Kiểm tra Debug Output
   → Restart MAUI app

2️⃣ Port 5000 đã dùng
   → Chạy Web Admin port khác:
     dotnet run --urls "http://localhost:5001"

3️⃣ Thêm quán từ Web nhưng MAUI không hiển thị
   → Refresh MAUI app
   → Kiểm tra console log
   → Hoặc restart MAUI

4️⃣ "Connection refused"
   → Web Admin chưa chạy
   → Port không chính xác
   → Firewall chặn

═══════════════════════════════════════════════════════════════════

✨ READY! Bắt đầu chạy nào! 🚀
