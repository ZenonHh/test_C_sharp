# 🍴 Vĩnh Khánh Tour - Admin Web Dashboard

Ứng dụng quản lý admin cho dự án Vĩnh Khánh Food Tour.

## 🚀 Cách chạy nhanh nhất

### 1️⃣ **Chạy Web Admin (ASP.NET Core)**

```bash
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run --urls "http://localhost:5000"
```

Web Admin sẽ chạy tại: **http://localhost:5000**

### 2️⃣ **Chạy MAUI App** (khi cần test kèm)

```bash
cd ..\..
dotnet build -f net8.0-android
```

## 📍 Database Location

Mặc định, app tìm kiếm database `VinhKhanhTour_Full.db3` tại:
- **Windows**: `C:\Users\{USERNAME}\AppData\Roaming\VinhKhanhTour\`
- **Có thể cấu hình**: Đặt biến môi trường `VINHKHANH_DB_PATH`

**Để XAMPP tìm thấy database:**
```powershell
[Environment]::SetEnvironmentVariable("VINHKHANH_DB_PATH", "C:\xampp\htdocs", "User")
```

Sau đó copy file database vào folder đó.

## 📊 Features

✅ **Quản lý quán ăn** (CRUD)
✅ **Quản lý người dùng** (CRUD)
✅ **Xem lịch sử phát**
✅ **API RESTful** - dùng cùng database SQLite

## 🔌 API Endpoints

### Quán Ăn (POI)
- `GET /api/pois` - Danh sách tất cả
- `GET /api/pois/{id}` - Chi tiết
- `POST /api/pois` - Tạo mới
- `PUT /api/pois/{id}` - Cập nhật
- `DELETE /api/pois/{id}` - Xóa

### Người Dùng
- `GET /api/users` - Danh sách
- `GET /api/users/{id}` - Chi tiết
- `POST /api/users` - Tạo mới
- `PUT /api/users/{id}` - Cập nhật
- `DELETE /api/users/{id}` - Xóa

### Lịch Sử
- `GET /api/history` - Danh sách
- `DELETE /api/history/{id}` - Xóa

## 🎯 Dùng với XAMPP

Nếu cần chạy trên XAMPP:
1. Copy `DoAnCSharp.AdminWeb` folder vào XAMPP
2. Chạy: `dotnet run --urls "http://localhost:80"`
3. Truy cập: http://localhost

## 📝 Notes

- Database được shared với MAUI app (cùng file SQLite)
- CORS đã enable - có thể gọi API từ các domain khác
- UI là single-page HTML + Vanilla JavaScript
- Không cần database migration - tự tạo tables nếu chưa có
