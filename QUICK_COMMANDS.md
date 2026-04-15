# Copy-paste commands để chạy nhanh

## 🚀 TERMINAL 1 - Web Admin

```powershell
# Cách 1 (Nhanh nhất - Chạy .bat file)
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
.\run-admin.bat

# HOẶC Cách 2 (Chạy dotnet trực tiếp)
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run

# HOẶC Cách 3 (Chạy trên port khác nếu 5000 bận)
dotnet run --urls "http://localhost:5001"
```

👉 Sau đó mở browser: http://localhost:5000

---

## 🚀 TERMINAL 2 - MAUI App

```powershell
# Từ thư mục gốc project
cd C:\Users\LENOVO\source\repos\do_an_C_sharp

# Chạy MAUI app
dotnet run -f net8.0-android
```

---

## 📋 Full Setup (Cả 2 một lúc)

```powershell
# Mở 2 PowerShell terminal cùng lúc, sau đó:

# Terminal 1:
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
.\run-admin.bat

# Terminal 2 (Chờ Web Admin chạy):
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
dotnet run -f net8.0-android
```

---

## 🔍 Check & Debug

```powershell
# Kiểm tra port 5000 có đang dùng không
netstat -ano | findstr :5000

# Kill process trên port 5000
taskkill /PID {PID} /F

# Clear build cache
dotnet clean

# Rebuild project
dotnet build

# Chạy riêng Web Admin (để debug)
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run

# Chạy riêng MAUI (để debug)
cd ..
cd ..
dotnet run -f net8.0-android
```

---

## 📚 Các file hướng dẫn

- `FINAL_SUMMARY_KET_NOI.txt` - Tổng quan hoàn chỉnh
- `HUONG_DAN_CHAY_STEP_BY_STEP.md` - Chi tiết từng bước
- `SETUP_CONNECTED_COMPLETE.txt` - Kiến trúc & kết nối
- `ADMIN_SETUP.md` - Setup Web Admin
- `🚀_START_HERE.txt` - Quick start

---

## 💡 Tips

1. **Chạy Web Admin TRƯỚC MAUI App**
   - Web Admin cung cấp API
   - MAUI cần kết nối đến Web Admin

2. **Không đóng Terminal Web Admin**
   - Nếu đóng → MAUI fallback sang local DB
   - MAUI vẫn chạy được nhưng không sync

3. **Để Demo**
   - Mở cả 2 app
   - Thêm/Sửa/Xóa từ Web Admin
   - Refresh MAUI để thấy thay đổi

---

## 🎯 Demo Live

```bash
# Terminal 1 - Web Admin API
run-admin.bat
# Output: "Now listening on: http://localhost:5000"

# Terminal 2 - MAUI Mobile App  
dotnet run -f net8.0-android
# Output: "Application started"

# Browser: http://localhost:5000
# = Admin Dashboard (Add/Edit/Delete)

# MAUI Phone Emulator:
# = Mobile App (View data từ API)

# Test:
# 1. Thêm quán mới từ Web Admin
# 2. Refresh MAUI App
# 3. Quán mới hiển thị ✓ = Kết nối thành công!
```
