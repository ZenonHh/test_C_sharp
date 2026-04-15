# 🏃 HƯỚNG DẪN CHẠY NHANH - WEB ADMIN

## ⚡ Cách 1: Chạy bằng Script (Nhanh nhất)

### Windows:
```
run-admin.bat
```

### Linux/Mac:
```
bash run-admin.sh
```

---

## ⚡ Cách 2: Chạy bằng Command Line

```powershell
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

Mở browser: **http://localhost:5000**

---

## 📍 Nếu dùng XAMPP

### Bước 1: Set đường dẫn database
```powershell
$env:VINHKHANH_DB_PATH = "C:\xampp\htdocs"
```

### Bước 2: Copy database vào XAMPP
```powershell
Copy-Item "C:\Users\{USERNAME}\AppData\Roaming\VinhKhanhTour\VinhKhanhTour_Full.db3" `
    "C:\xampp\htdocs\"
```

### Bước 3: Chạy trên port 80 (nếu muốn)
```powershell
dotnet run --urls "http://localhost:80"
```

---

## 🎯 Chạy cùng MAUI App

1. Chạy MAUI app bình thường
2. Chạy Web Admin (port khác, vd: 5001)
3. Cả 2 chia sẻ cùng database SQLite

---

## ✅ Kiểm tra

Nếu thấy:
```
Now listening on: http://localhost:5000
```

✅ Thành công! Mở http://localhost:5000 trong browser

---

## 🔧 Troubleshooting

**Lỗi port đang dùng:**
```powershell
dotnet run --urls "http://localhost:5001"
```

**Không tìm thấy database:**
- Chắc chắn MAUI app đã chạy 1 lần (để tạo database)
- Hoặc set biến `VINHKHANH_DB_PATH` chỉ tới folder có database

---

## 📊 Features

✅ Quản lý quán ăn (Add/Edit/Delete)
✅ Quản lý người dùng
✅ Xem lịch sử phát
✅ API RESTful tương thích với MAUI app
