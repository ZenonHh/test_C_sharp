# 📱 Hệ Thống Quản Lý Tour Du Lịch Vĩnh Khánh - Hướng Dẫn Sử Dụng

## ✅ Tổng Quan Hệ Thống

Hệ thống bao gồm 3 phần chính:
1. **Mobile App (MAUI)** - Dành cho du khách
2. **Web Admin** - Quản lý hệ thống
3. **Public Web** - Hiển thị thông tin quán ăn từ QR

---

## 🎯 1. WEB ADMIN DASHBOARD

### 📍 Truy Cập
```
http://localhost:5000/
hoặc
http://localhost:5000/index.html
```

### 🏠 Dashboard (Tổng Quan)
**Chức năng:**
- Hiển thị thống kê realtime:
  - 🟢 Người Online
  - 🎧 Đang Nghe
  - 👤 Tổng Users
  - 💳 Thanh Toán
  - 📊 QR Hôm Nay
- Danh sách người dùng online
- Hoạt động quét QR

---

### 🏪 Quản Lý Quán Ăn (POIs)

#### A. Hiển Thị
- **Layout:** Grid 2 cột (2x3 = 6 quán/trang)
- **Phân trang:** Tự động khi > 6 quán

#### B. Thông Tin Mỗi Quán
```
┌──────────────────────┐
│  📱 QR Icon          │
├──────────────────────┤
│  Tên Quán            │
│  📍 Địa Chỉ          │
│  Mô Tả               │
│  QR: POI_ABC123...   │
│  QR: ✅ Có           │
├──────────────────────┤
│ [👁️ QR] [✏️ Sửa] [🗑️]│
└──────────────────────┘
```

#### C. Thêm Quán Mới
1. Click **➕ Thêm Quán Ăn Mới**
2. Điền thông tin:
   - Tên Quán *
   - Địa Chỉ *
   - Mô Tả
   - Vĩ Độ (Latitude) *
   - Kinh Độ (Longitude) *
   - Bán Kính (mặc định: 50m)
   - Ưu Tiên (mặc định: 1)
   - **Mã QR Code**
     - Để trống → Tự động sinh
     - Click **🔄 Tạo** → Tạo mã mới
3. Click **✅ Thêm Quán**

#### D. Xem QR Code
- Click **👁️ QR** → Mở trang công khai trong tab mới
- URL: `/poi-public.html?qr=POI_ABC123`

---

### 📱 Quản Lý Thiết Bị

#### A. Hiển Thị
- **Modes:** Card View / Table View
- **Phân trang:** 5 thiết bị/trang
- **Filters:**
  - 🟢 Tất Cả
  - 🟢 Online
  - 🔴 Offline

#### B. Tìm Kiếm
- Realtime search
- Tìm theo:
  - Tên thiết bị
  - Model
  - IP Address
  - OS

#### C. Thông Tin Thiết Bị (Card)
```
┌──────────────────────────┐
│ Device Name    [🟢 Online]│
├──────────────────────────┤
│ Model: Samsung A12       │
│ OS: Android              │
│ App: 1.2.3               │
│ IP: 192.168.1.100        │
│ Cuối: 15/04/2026 14:30   │
│ 📍 Hà Nội, Việt Nam      │
├──────────────────────────┤
│ [✓ Kích Hoạt] [🗑️ Xóa]  │
└──────────────────────────┘
```

#### D. Thống Kê
```
┌─────────┬─────────┬─────────┐
│ Online  │ Offline │  Tổng   │
│    3    │    2    │    5    │
└─────────┴─────────┴─────────┘
```

#### E. Hành Động
- **✓ Kích Hoạt / ✗ Vô Hiệu** - Bật/tắt thiết bị
- **🗑️ Xóa** - Xóa thiết bị khỏi hệ thống

---

### 👥 Quản Lý Người Dùng
- Danh sách users
- Trạng thái: 🟢 Online | 🔴 Offline
- Thanh toán: 💳 Paid | 🆓 Free

---

### 💳 Quản Lý Thanh Toán
- Thêm/Cập nhật thanh toán
- Theo dõi người dùng trả phí

---

### 📊 QR Scan Limits
- Giới hạn số lần quét QR mỗi thiết bị
- Reset hàng ngày

---

### 📜 Lịch Sử Phát
- Theo dõi lịch sử nghe audio của users

---

## 🌐 2. TRANG CÔNG KHAI (POI PUBLIC)

### 📍 Truy Cập
```
http://localhost:5000/poi-public.html?qr=POI_ABC123
```

### 🎯 Chức Năng
1. **Hiển thị thông tin quán:**
   - Tên quán
   - Địa chỉ
   - Mô tả
   - Bán kính phục vụ
   - Ưu tiên

2. **Thống kê realtime:**
   - 🟢 Người Online Hiện Tại

3. **Call-to-Action:**
   - 📱 **Tải Ứng Dụng Vĩnh Khánh Tour**
   - 🎉 **Tôi Đã Tới Quán**

4. **Tự động:**
   - Ghi nhận quét QR
   - Cộng +1 online count
   - Update dashboard realtime

---

## 📲 3. MOBILE APP (MAUI)

### Tính Năng Chính
1. **Quét QR Code**
   - Scan QR từ quán ăn
   - Gửi lên server
   - Server verify & increment online count

2. **Xem Bản Đồ**
   - Hiển thị quán ăn gần đó

3. **Nghe Audio Hướng Dẫn**
   - Tự động phát khi vào vùng radius

4. **Quản Lý Profile**
   - Xem lịch sử
   - Cài đặt

---

## 🔌 4. API ENDPOINTS

### A. POI Endpoints
```http
GET  /api/pois              # Lấy tất cả quán
GET  /api/pois/{id}         # Lấy quán theo ID
GET  /api/pois/qr/{qrCode}  # Lấy quán theo QR code
POST /api/pois              # Tạo quán mới
PUT  /api/pois/{id}         # Cập nhật quán
DELETE /api/pois/{id}       # Xóa quán
```

### B. Device Endpoints
```http
GET  /api/devices                        # Lấy tất cả thiết bị
GET  /api/devices/{id}                   # Lấy thiết bị theo ID
GET  /api/devices/stats                  # Thống kê thiết bị
POST /api/devices                        # Đăng ký thiết bị mới
PUT  /api/devices/{id}                   # Cập nhật thiết bị
PUT  /api/devices/{id}/toggle            # Bật/tắt thiết bị
DELETE /api/devices/{id}                 # Xóa thiết bị
POST /api/devices/status/online/increment # Cộng online count
```

### C. User Endpoints
```http
GET  /api/users                    # Lấy tất cả users
GET  /api/users/{id}               # Lấy user theo ID
GET  /api/users/dashboard/summary  # Thống kê dashboard
POST /api/users                    # Tạo user mới
PUT  /api/users/{id}               # Cập nhật user
DELETE /api/users/{id}             # Xóa user
```

---

## 🧪 5. CÁCH TEST HỆ THỐNG

### Test 1: Thêm Quán Ăn với QR
1. Vào Web Admin
2. Tab **Quán Ăn**
3. Click **➕ Thêm Quán Ăn Mới**
4. Điền:
   ```
   Tên: Quán Ốc J24
   Địa chỉ: 24 Đường Cư Xá Vĩnh Hội Phường 6, Khánh Hội TPHCM
   Mô tả: Quán ốc ngon
   Lat: 10.762622
   Lng: 106.660172
   QR: (Để trống hoặc click Tạo)
   ```
5. Click **✅ Thêm Quán**
6. Verify:
   - Quán hiển thị trong grid
   - QR code hiển thị
   - Click **👁️ QR** → Mở trang công khai

### Test 2: Quét QR từ Mobile
1. Mở app MAUI
2. Scan QR code
3. Verify:
   - App nhận QR
   - Server ghi nhận
   - Online count +1 trên dashboard

### Test 3: Quản Lý Thiết Bị
1. Vào tab **Thiết Bị**
2. Verify:
   - Hiển thị thiết bị (nếu có)
   - Filter Online/Offline hoạt động
   - Search hoạt động
   - Pagination hiển thị khi > 5 devices

### Test 4: Pagination
**POI:**
- Thêm > 6 quán
- Verify pagination xuất hiện
- Click trang 2 → Hiển thị 6 quán tiếp theo

**Devices:**
- Cần > 5 thiết bị
- Verify pagination 5 items/page

---

## 💾 6. CẤU TRÚC DATABASE

### Tables
1. **AudioPOI** - Quán ăn
   - QRCode (string) - Mã QR unique

2. **UserDevice** - Thiết bị
   - IsOnline (bool)
   - LastOnlineAt (DateTime)
   - DeviceInfo, IP, Location...

3. **User** - Người dùng
   - Email, Phone, IsPaid...

4. **UserPayment** - Thanh toán
5. **PlayHistory** - Lịch sử phát
6. **QRCodeSession** - Session QR
7. **DeviceScanLimit** - Giới hạn quét

---

## ⚙️ 7. CẤU HÌNH

### Port
- **AdminWeb:** 5000
- **Mobile API:** Same endpoint

### Auto-Refresh
- **Dashboard:** 10 giây
- **Devices:** 5 giây
- **POI Public:** 5 giây

### Phân Trang
- **POI:** 6 items/page (Grid 2x3)
- **Devices:** 5 items/page

---

## 🎨 8. STYLE & THEME

### Colors
```css
--primary: #667eea
--secondary: #764ba2
--success: #27ae60
--danger: #e74c3c
--warning: #f39c12
--info: #3498db
```

### Layout
- **POI Grid:** 2 columns
- **Devices Grid:** Auto-fill (min 300px)
- **Responsive:** Mobile-friendly

---

## 🚀 9. CHẠY HỆ THỐNG

### AdminWeb
```powershell
cd DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb
dotnet run
```
Truy cập: http://localhost:5000

### MAUI App
```powershell
dotnet build -f net8.0-android
dotnet run -f net8.0-android
```

---

## 📝 10. LƯU Ý QUAN TRỌNG

### 1. QR Code
- Tự động sinh nếu không nhập
- Format: `POI_XXXXXXX`
- Unique constraint trong DB

### 2. Online Count
- Realtime update
- Reset khi server restart (in-memory)
- Để persistent → Lưu vào DB

### 3. Device Tracking
- Ghi nhận khi app online
- Update LastOnlineAt
- Auto mark offline sau X phút (TODO)

### 4. Performance
- Pagination giảm load
- Auto-refresh an toàn
- Cache nếu cần

---

## 🎓 11. ĐIỂM MẠNH CHO THESIS

✅ **Realtime System**
- Dashboard tự động cập nhật
- Online tracking
- QR scan integration

✅ **Modern Tech Stack**
- .NET 8 MAUI
- ASP.NET Core Web API
- SQLite Database

✅ **Full Features**
- Mobile App
- Web Admin
- Public Pages
- API Backend

✅ **User Experience**
- Beautiful UI
- Responsive design
- Realtime feedback
- Pagination & Search

✅ **Scalability**
- Modular architecture
- RESTful API
- Separated concerns

---

## 🔐 12. BẢO MẬT

- ✅ Admin authentication (TODO: implement)
- ✅ API validation
- ✅ QR expiry (5 phút)
- ✅ Device limits

---

## 📞 13. SUPPORT

**Vấn đề thường gặp:**

1. **"Đang tải..." không dứt**
   - Check API endpoint
   - Check network tab
   - Verify database có dữ liệu

2. **Pagination không hiện**
   - Cần > 6 quán (POI)
   - Cần > 5 thiết bị (Devices)

3. **QR không hiển thị**
   - Check poi.qrCode có giá trị
   - Click "👁️ QR" để xem trang công khai

---

**Good luck với thesis! 🎓✨**

**Hệ thống đã sẵn sàng để demo và triển khai!**
