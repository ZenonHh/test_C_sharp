# ⚡ SETUP DEV TUNNELS - 5 PHÚT

## 🚀 **Cách nhanh nhất (Visual Studio 2022)**

### **Bước 1: Mở Project**
```
File → Open → Project/Solution
→ Chọn: DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb.csproj
```

### **Bước 2: Start với Dev Tunnels**

1. **Click dropdown** nút ▶️ Run (phía trên toolbar)
   
2. **Chọn "Dev Tunnels"** từ dropdown

3. **Nếu chưa có tunnel:**
   - Click **"Create a Tunnel"**
   - **Name:** `vinhkhanh-tour`
   - **Type:** Persistent
   - **Access:** Public
   - Click **Create**

4. **Nhấn ▶️ Run** (hoặc F5)

### **Bước 3: Lấy Public URL**

Sau khi server chạy, xem **Output** window (View → Output):

```
Dev Tunnels: Tunnel is ready!
Public URL: https://vinhkhanh-tour-abc123.devtunnels.ms
```

**Copy URL này!**

### **Bước 4: Test QR**

**Mở trình duyệt (bất kỳ thiết bị nào):**
```
https://vinhkhanh-tour-abc123.devtunnels.ms
```

**Tạo QR test:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
```

**Quét QR từ điện thoại** (bất kỳ mạng nào) → ✅ **Thành công!**

---

## 🔧 **Cách 2: PowerShell Script (Nếu không dùng VS 2022)**

### **Terminal 1 - Chạy Server:**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

### **Terminal 2 - Setup Tunnel:**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb
.\setup-dev-tunnel.ps1
```

Script sẽ tự động:
- Kiểm tra devtunnel CLI
- Đăng nhập (nếu chưa)
- Tạo tunnel `vinhkhanh-tour`
- Kết nối port 5000
- Hiển thị public URL

**Copy URL và dùng ngay!**

---

## ✅ **Verification**

**1. Kiểm tra server local:**
```
http://localhost:5000
```

**2. Kiểm tra tunnel URL:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms
```

**3. Test QR scan:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
```

**4. Quét bằng điện thoại (bất kỳ mạng):**
- Mở Camera
- Quét QR code
- ✅ Trang hiển thị ngay!

---

## 🐛 **Gặp lỗi?**

### **Lỗi: "devtunnel not found"**
```powershell
winget install Microsoft.devtunnel
```

### **Lỗi: "Not logged in"**
```powershell
devtunnel user login
```

### **Lỗi: "Port already in use"**
```powershell
# Tìm process đang dùng port 5000
netstat -ano | findstr :5000

# Kill process
taskkill /PID <PID> /F
```

---

## 📱 **Tạo QR mới với Tunnel URL**

**Cũ (chỉ hoạt động cùng WiFi):**
```
http://192.168.1.100:5000/qr/POI_UA8AG0H2D
```

**Mới (hoạt động ở bất kỳ đâu):**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
```

**Cách tạo tự động:**
1. Vào Admin Dashboard
2. Tab **Quán Ăn**
3. Click **Sửa** trên POI bất kỳ
4. Click **Tạo mã QR**
5. QR tự động dùng tunnel URL

---

## 🎯 **Kết quả**

✅ **Server chạy local:** `http://localhost:5000`
✅ **Public URL:** `https://vinhkhanh-tour-abc123.devtunnels.ms`
✅ **Quét QR từ bất kỳ đâu:** Không cần cùng WiFi
✅ **HTTPS tự động:** Bảo mật
✅ **Persistent URL:** Không đổi sau restart

---

**🎉 Xong! Giờ có thể quét QR từ bất kỳ đâu trên thế giới!**
