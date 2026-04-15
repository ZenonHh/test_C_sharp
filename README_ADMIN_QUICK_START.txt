╔════════════════════════════════════════════════════════════════╗
║     ✅ WEB ADMIN ĐÀNH HOÀN THÀNH - SẴN CHẠY                    ║
╚════════════════════════════════════════════════════════════════╝

📦 ĐÃ TẠO:
  ✓ DoAnCSharp.AdminWeb/ - ASP.NET Core Web project
  ✓ API Endpoints CRUD (POI, Users, History)
  ✓ HTML5 Admin Dashboard (Vanilla JS, không cần Blazor)
  ✓ Share Database SQLite giữa MAUI & Web

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🚀 CHẠY NGAY (3 cách):

[Cách 1] Windows - Script nhanh nhất:
  > run-admin.bat
  
[Cách 2] PowerShell:
  > cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
  > dotnet run
  
[Cách 3] Custom port:
  > dotnet run --urls "http://localhost:5001"

👉 Mở browser: http://localhost:5000

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🎯 FEATURES HOÀN THÀNH:

✅ QUẢN LÝ QUÁN ĂN (AudioPOI)
   - Xem danh sách (16 quán hiện có)
   - Thêm quán mới (Form + Map coordinates)
   - Sửa thông tin quán
   - Xóa quán

✅ QUẢN LÝ NGƯỜI DÙNG
   - Xem danh sách users
   - Thêm user mới
   - Sửa profile
   - Xóa user

✅ LỊCH SỬ PHÁT
   - Xem lịch sử nghe audio
   - Xóa lịch sử
   - Sắp xếp theo thời gian

✅ API RESTFUL
   GET    /api/pois
   GET    /api/pois/{id}
   POST   /api/pois
   PUT    /api/pois/{id}
   DELETE /api/pois/{id}
   
   GET    /api/users
   GET    /api/users/{id}
   POST   /api/users
   PUT    /api/users/{id}
   DELETE /api/users/{id}
   
   GET    /api/history
   DELETE /api/history/{id}

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

💾 DATABASE:

Database file: VinhKhanhTour_Full.db3
Vị trí mặc định: C:\Users\{USERNAME}\AppData\Roaming\VinhKhanhTour\

CÙNG DATABASE với MAUI app:
✓ MAUI app tạo và nạp dữ liệu
✓ Web Admin quản lý dữ liệu
✓ Không cần migrate hay sync

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🔌 DÙNG VỚI XAMPP (Nếu cần):

1. Set environment variable:
   $env:VINHKHANH_DB_PATH = "C:\xampp\htdocs"

2. Copy database:
   Copy "C:\Users\{USERNAME}\AppData\Roaming\VinhKhanhTour\VinhKhanhTour_Full.db3" 
        "C:\xampp\htdocs\"

3. Chạy trên port 80:
   dotnet run --urls "http://localhost:80"

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📁 FILE STRUCTURE:

DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb\
├── Models\
│   ├── AudioPOI.cs
│   ├── User.cs
│   └── PlayHistory.cs
├── Services\
│   └── DatabaseService.cs
├── wwwroot\
│   └── index.html          (Admin Dashboard UI)
├── Program.cs              (API Endpoints)
└── DoAnCSharp.AdminWeb.csproj

run-admin.bat              (Quick start script)
run-admin.sh               (Linux/Mac)
HUONG_DAN_CHAY_ADMIN.md    (Hướng dẫn chi tiết)
ADMIN_SETUP.md             (Setup documentation)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

❓ TROUBLESHOOTING:

❌ Port 5000 đang dùng?
   → Chạy: dotnet run --urls "http://localhost:5001"

❌ Không tìm thấy database?
   → MAUI app chưa chạy? Chạy MAUI 1 lần để tạo DB
   → Hoặc set VINHKHANH_DB_PATH environment variable

❌ CORS error gọi API?
   → CORS đã enable trong Program.cs, refresh browser cache

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📅 HƯỚNG DẪN CHO BUỔI CHẤM ĐỒ ÁN:

1. Bật XAMPP (nếu cần Apache + MySQL cho phần khác)

2. Chạy MAUI app:
   > cd C:\Users\LENOVO\source\repos\do_an_C_sharp
   > dotnet run -f net8.0-android

3. Chạy Web Admin (terminal khác):
   > run-admin.bat

4. Trình bày:
   ✓ MAUI Mobile App (quân ánh, map, profile)
   ✓ Web Admin Dashboard (quản lý dữ liệu)
   ✓ Shared SQLite Database
   ✓ API RESTful

5. Demo:
   - Thêm quán ăn mới từ Web Admin
   - Refresh MAUI app → Quán mới hiển thị
   - Hoặc ngược lại: MAUI → Web Admin

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✨ Mọi thứ sẵn sàng! Chúc báo cáo thành công!

Nếu cần chỉnh sửa gì, edit file trong:
  DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb\

Rebuild: dotnet build
Chạy lại: dotnet run
