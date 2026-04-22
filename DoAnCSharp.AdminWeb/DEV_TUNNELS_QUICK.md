# 🚀 QUICK START - Dev Tunnels

## ⚡ **Cách nhanh nhất (5 phút)**

### **Phương pháp 1: Visual Studio 2022 (Khuyến nghị)**

1. **Mở project** trong Visual Studio 2022
2. **Click dropdown** nút Run → Chọn **"Dev Tunnels"**
3. **Create Tunnel:**
   - Name: `vinhkhanh-tour`
   - Type: Persistent
   - Access: Public
4. **Nhấn Run** → Server tự động chạy với tunnel
5. **Copy URL** từ Output window (dạng: `https://abc123.devtunnels.ms`)

✅ **Xong!** Bạn có thể quét QR từ bất kỳ đâu với URL này!

---

### **Phương pháp 2: PowerShell Script (Tự động)**

**Mở 2 terminals:**

**Terminal 1 - Chạy server:**
```powershell
cd DoAnCSharp.AdminWeb
dotnet run
```

**Terminal 2 - Setup tunnel:**
```powershell
cd DoAnCSharp.AdminWeb
.\setup-dev-tunnel.ps1
```

Script sẽ tự động:
- Tạo tunnel (nếu chưa có)
- Cấu hình port 5000
- Kết nối tunnel
- Hiển thị public URL

✅ **Xong!** Copy URL và dùng ngay!

---

## 🌐 **URL bạn sẽ nhận được**

```
https://vinhkhanh-tour-abc123.devtunnels.ms
```

**Thay thế tất cả QR code cũ:**
- ❌ Cũ: `http://192.168.1.100:5000/qr/POI_UA8AG0H2D`
- ✅ Mới: `https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D`

---

## 📱 **Test QR Code**

1. **Tạo QR mới** với tunnel URL (dùng Admin Dashboard)
2. **In hoặc hiển thị** QR trên màn hình
3. **Quét bằng camera điện thoại** (bất kỳ mạng nào)
4. ✅ Trang hiển thị thông tin quán ngay lập tức!

---

## 🐛 **Gặp lỗi?**

### **Lỗi 1: "devtunnel not found"**
```powershell
winget install Microsoft.devtunnel
```

### **Lỗi 2: "Not logged in"**
```powershell
devtunnel user login
```

### **Lỗi 3: "Port already in use"**
```powershell
# Tìm process đang dùng port 5000
netstat -ano | findstr :5000

# Kill process
taskkill /PID <PID> /F
```

---

## 📚 **Tài liệu đầy đủ**

Xem chi tiết trong: `DEV_TUNNELS_GUIDE.md`

---

**🎉 Chúc bạn thành công!**

Giờ đây bạn có thể quét QR từ **bất kỳ đâu** trên thế giới! 🌍
