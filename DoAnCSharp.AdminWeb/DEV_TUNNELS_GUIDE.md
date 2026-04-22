# 🌐 Hướng dẫn Dev Tunnels - Quét QR từ Internet (Không cần cùng mạng WiFi)

## 📖 **Giới thiệu**

**Dev Tunnels** (Visual Studio Tunnels) cho phép bạn **công khai server local** ra Internet, giúp:
- ✅ Quét QR từ điện thoại **không cần cùng mạng WiFi**
- ✅ Chia sẻ server với người khác **ở bất kỳ đâu**
- ✅ Test trên thiết bị di động **thực tế** (iOS/Android)
- ✅ **Hoàn toàn miễn phí** với Visual Studio 2022

---

## 🚀 **Phương pháp 1: Sử dụng Visual Studio 2022 (Khuyến nghị)**

### **Bước 1: Cài đặt Visual Studio 2022**

Đảm bảo bạn có:
- ✅ **Visual Studio 2022 version 17.4** trở lên
- ✅ Workload: **ASP.NET and web development**

### **Bước 2: Mở Project và Start với Dev Tunnels**

1. **Mở project** `DoAnCSharp.AdminWeb.csproj` trong Visual Studio 2022

2. **Click vào dropdown** của nút Run (thường là `https` hoặc `IIS Express`)

3. **Chọn "Dev Tunnels"** → **Create a Tunnel**

4. **Cấu hình Tunnel:**
   ```
   Tunnel Name: vinhkhanh-tour
   Tunnel Type: Persistent
   Access: Public (để ai cũng có thể truy cập)
   ```

5. **Nhấn Create** → Visual Studio sẽ tạo tunnel và cho bạn **URL công khai**

### **Bước 3: Nhận URL Tunnel**

Sau khi create, bạn sẽ có URL dạng:
```
https://vinhkhanh-tour-abc123.devtunnels.ms
```

Đây là URL công khai, **bất kỳ ai** có link này đều truy cập được!

### **Bước 4: Tạo QR Code với URL Tunnel**

**Ví dụ POI:** Ốc Oanh có ID = 1, QR code cũ:
```
http://192.168.1.100:5000/qr/POI_UA8AG0H2D
```

**QR code mới với tunnel:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
```

**Cách tạo QR tự động:**

1. Vào **Admin Dashboard** → Tab **Địa điểm (POIs)**
2. Click **"Sửa"** trên POI bất kỳ
3. Click **"Tạo mã QR"** (nút màu xanh dương)
4. QR code sẽ tự động sử dụng tunnel URL nếu đã kích hoạt

### **Bước 5: Test QR từ điện thoại**

1. **In QR code** hoặc hiển thị trên màn hình
2. Mở **Camera** trên điện thoại (không cần app đặc biệt)
3. **Quét QR** → Trình duyệt tự động mở với tunnel URL
4. ✅ **Thành công!** Trang thông tin quán hiển thị

---

## 🌍 **Phương pháp 2: Sử dụng devtunnel CLI (Command Line)**

### **Bước 1: Cài đặt devtunnel CLI**

**Windows (PowerShell):**
```powershell
# Download devtunnel CLI
winget install Microsoft.devtunnel
```

**Hoặc tải thủ công:**
- Link: https://aka.ms/devtunnels/download
- Giải nén vào thư mục, thêm vào PATH

### **Bước 2: Đăng nhập**

```powershell
devtunnel user login
```

Sử dụng Microsoft Account (cùng account với Visual Studio)

### **Bước 3: Tạo Tunnel**

```powershell
# Tạo tunnel persistent (lưu lại sau khi tắt)
devtunnel create vinhkhanh-tour --allow-anonymous

# Kết quả:
# Tunnel ID: abc123
# Tunnel URI: vinhkhanh-tour-abc123.devtunnels.ms
```

### **Bước 4: Chạy Server và Kết nối Tunnel**

**Terminal 1 - Chạy server:**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

**Terminal 2 - Kết nối tunnel:**
```powershell
# Port server đang chạy (VD: 5000)
devtunnel port create abc123 -p 5000

# Kích hoạt tunnel
devtunnel host abc123
```

Kết quả:
```
✅ Tunnel is ready!
Public URL: https://vinhkhanh-tour-abc123.devtunnels.ms
```

### **Bước 5: Test**

Mở trình duyệt trên **bất kỳ thiết bị nào** (không cần cùng mạng):
```
https://vinhkhanh-tour-abc123.devtunnels.ms
```

✅ **Admin Dashboard** hiển thị bình thường!

---

## ⚙️ **Phương pháp 3: Ngrok (Thay thế Dev Tunnels)**

Nếu gặp vấn đề với Dev Tunnels, dùng **ngrok**:

### **Bước 1: Cài đặt Ngrok**

```powershell
# Download: https://ngrok.com/download
# Hoặc dùng Chocolatey
choco install ngrok
```

### **Bước 2: Đăng ký và lấy Auth Token**

1. Đăng ký tại: https://dashboard.ngrok.com/signup
2. Copy **Authtoken** từ dashboard
3. Chạy:
   ```powershell
   ngrok config add-authtoken YOUR_AUTH_TOKEN
   ```

### **Bước 3: Chạy Server**

```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

### **Bước 4: Kết nối Ngrok Tunnel**

**Terminal mới:**
```powershell
ngrok http 5000
```

Kết quả:
```
Forwarding: https://abc123.ngrok-free.app → http://localhost:5000
```

### **Bước 5: Test**

Dùng URL công khai từ ngrok:
```
https://abc123.ngrok-free.app/qr/POI_UA8AG0H2D
```

---

## 📋 **So sánh các phương pháp**

| Tính năng | Dev Tunnels (VS) | devtunnel CLI | Ngrok |
|-----------|------------------|---------------|-------|
| **Miễn phí** | ✅ Vĩnh viễn | ✅ Vĩnh viễn | ⚠️ Giới hạn (60 phút/session) |
| **Persistent URL** | ✅ | ✅ | ❌ (mỗi lần chạy khác URL) |
| **Tốc độ** | ⚡ Nhanh | ⚡ Nhanh | ⚡ Nhanh |
| **Dễ sử dụng** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **Tích hợp VS** | ✅ | ❌ | ❌ |
| **HTTPS** | ✅ | ✅ | ✅ |

**Khuyến nghị:** Dùng **Dev Tunnels trong Visual Studio 2022** (dễ nhất)

---

## 🔧 **Cấu hình nâng cao**

### **1. Tự động tạo QR với Tunnel URL**

File: `DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb/Services/DatabaseService.cs`

Thêm method:
```csharp
public async Task<string> GetCurrentBaseUrlAsync()
{
    // Check if running behind dev tunnel
    var tunnelUrl = Environment.GetEnvironmentVariable("DEV_TUNNEL_URL");
    if (!string.IsNullOrEmpty(tunnelUrl))
        return tunnelUrl;
    
    // Default local URL
    return "http://172.20.10.2:5000";
}

public async Task<string> GenerateQRCodeUrlAsync(AudioPOI poi)
{
    var baseUrl = await GetCurrentBaseUrlAsync();
    return $"{baseUrl}/qr/{poi.QRCode}";
}
```

### **2. Cấu hình CORS cho Dev Tunnels**

File: `Program.cs`

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDevTunnel", policy =>
    {
        policy.WithOrigins("https://*.devtunnels.ms")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowDevTunnel");
```

### **3. Lưu Tunnel URL vào Environment Variable**

**Windows PowerShell:**
```powershell
# Tạm thời (session hiện tại)
$env:DEV_TUNNEL_URL = "https://vinhkhanh-tour-abc123.devtunnels.ms"

# Vĩnh viễn (system-wide)
[System.Environment]::SetEnvironmentVariable("DEV_TUNNEL_URL", "https://vinhkhanh-tour-abc123.devtunnels.ms", "User")
```

**Linux/Mac:**
```bash
export DEV_TUNNEL_URL="https://vinhkhanh-tour-abc123.devtunnels.ms"
```

---

## 🐛 **Xử lý lỗi thường gặp**

### **Lỗi 1: "Tunnel not found"**

**Nguyên nhân:** Tunnel chưa được tạo hoặc đã bị xóa

**Giải pháp:**
```powershell
# Xem danh sách tunnels
devtunnel list

# Tạo lại
devtunnel create vinhkhanh-tour --allow-anonymous
```

### **Lỗi 2: "Access denied"**

**Nguyên nhân:** Tunnel không cho phép anonymous access

**Giải pháp:**
```powershell
devtunnel update <tunnel-id> --allow-anonymous
```

### **Lỗi 3: "Port already in use"**

**Nguyên nhân:** Port 5000 đã được sử dụng

**Giải pháp:**
```powershell
# Tìm process đang dùng port
netstat -ano | findstr :5000

# Kill process
taskkill /PID <PID> /F

# Hoặc dùng port khác
dotnet run --urls "http://0.0.0.0:5001"
```

### **Lỗi 4: "HTTPS certificate error"**

**Nguyên nhân:** Chứng chỉ dev không tin cậy

**Giải pháp:**
```powershell
# Trust dev certificate
dotnet dev-certs https --trust
```

---

## 📱 **Workflow hoàn chỉnh**

### **Scenario: Tạo QR cho quán mới**

1. **Chạy server với Dev Tunnels:**
   ```powershell
   # Visual Studio 2022 → Run → Dev Tunnels → Start
   ```

2. **Vào Admin Dashboard:**
   ```
   https://vinhkhanh-tour-abc123.devtunnels.ms
   ```

3. **Tạo POI mới:**
   - Tab: **Địa điểm (POIs)**
   - Click **"Thêm mới"**
   - Nhập: Tên, địa chỉ, mô tả
   - Click **"Lưu"**

4. **Tạo QR code:**
   - Click **"Sửa"** trên POI vừa tạo
   - Click **"Tạo mã QR"**
   - QR tự động sử dụng tunnel URL
   - Download PNG hoặc in trực tiếp

5. **Test:**
   - In QR code
   - Quét bằng điện thoại (bất kỳ mạng nào)
   - ✅ Trang hiển thị thông tin quán

---

## 🎯 **Kết luận**

**Các bước quan trọng:**

1. ✅ Cài Visual Studio 2022 (17.4+)
2. ✅ Mở project trong VS
3. ✅ Run → Dev Tunnels → Create Tunnel
4. ✅ Copy tunnel URL
5. ✅ Tạo QR với tunnel URL
6. ✅ Test trên điện thoại

**Lợi ích:**
- 🌍 Quét QR từ **bất kỳ đâu** (không cần cùng mạng)
- 🚀 **Miễn phí** và không giới hạn
- ⚡ **Tốc độ nhanh** (Microsoft Azure backbone)
- 🔒 **Bảo mật** với HTTPS tự động

---

## 📚 **Tài liệu tham khảo**

- **Dev Tunnels Official:** https://learn.microsoft.com/en-us/aspnet/core/test/dev-tunnels
- **Visual Studio Tunnels:** https://devblogs.microsoft.com/visualstudio/dev-tunnels/
- **Ngrok Documentation:** https://ngrok.com/docs
- **ASP.NET CORS:** https://learn.microsoft.com/en-us/aspnet/core/security/cors

---

**🎉 Chúc bạn thành công!**

Nếu gặp vấn đề, hãy kiểm tra:
1. Visual Studio 2022 version >= 17.4
2. Server đang chạy (dotnet run)
3. Tunnel đã được kích hoạt
4. QR code sử dụng đúng tunnel URL

---

**📝 Ghi chú:**
- Dev Tunnels hoạt động tốt nhất trên **Visual Studio 2022**
- Không cần cấu hình router/firewall
- Tunnel URL **persistent** (giữ nguyên sau khi restart)
- Tự động HTTPS, không cần chứng chỉ
