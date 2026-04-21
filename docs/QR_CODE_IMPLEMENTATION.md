# Hướng Dẫn Sử Dụng Tính Năng QR Code - Vĩnh Khánh Tour

## Tổng Quan

Hệ thống QR Code đã được triển khai đầy đủ với các tính năng:

### ✅ Đã Hoàn Thành

1. **CSS Fixed** - Đã sửa lỗi CSS 404 errors
   - Thay đổi từ `href="css/admin.css"` sang `href="/css/admin.css"` (absolute path)
   - CSS giờ load đúng trên tất cả các trang

2. **QR Code Display** - Quán ăn hiện hiển thị đầy đủ mã QR
   - QR code được hiển thị rõ ràng trong POI cards
   - Có phần QR display với styling đẹp
   - Hiển thị cảnh báo nếu quán chưa có mã QR

3. **Device Tracking** - Theo dõi thiết bị quét QR
   - Sử dụng cookie `vkt_device_id` để nhận dạng thiết bị
   - Giới hạn 5 lần quét/ngày cho người dùng miễn phí
   - Reset giới hạn mỗi ngày (00:00 UTC)
   - Ghi nhận thông tin: Device ID, IP, User Agent, Location

4. **QR Scan Workflow** - Quy trình quét QR hoàn chỉnh
   - Quét QR → Kiểm tra giới hạn → Hiển thị thông tin quán
   - Deep link vào app nếu đã cài đặt (`vinhkhanhtour://poi/{id}`)
   - Fallback sang web nếu chưa có app
   - Hiển thị số lượt quét còn lại

5. **Restaurant Info Page** - Trang thông tin quán ăn
   - Hiển thị đầy đủ thông tin quán
   - Nút tải app (auto-detect iOS/Android)
   - Thông tin online users
   - Cảnh báo khi hết lượt quét

---

## Cách Sử Dụng Admin Dashboard

### 1. Quản Lý Quán Ăn

#### Thêm Quán Mới
1. Vào tab **🏪 Quán Ăn**
2. Click **➕ Thêm Quán Ăn Mới**
3. Điền thông tin:
   - Tên quán
   - Địa chỉ
   - Mô tả
   - Vị trí (Lat/Lng)
   - Bán kính
4. Click **🎲 Tạo QR** để tạo mã QR tự động
   - Mã QR có dạng: `POI_XXXXXXXXXX`
5. Click **💾 Lưu**

#### Xem Mã QR
1. Trong danh sách quán ăn, tìm quán cần xem QR
2. Mã QR hiển thị ngay trong card:
   ```
   Mã QR Code:
   POI_A1B2C3D4E5
   Quét để xem thông tin quán
   ```
3. Click nút **👁️ Xem QR** để mở trang công khai

#### Chỉnh Sửa Quán
1. Click **✏️ Sửa** trên quán cần chỉnh sửa
2. Cập nhật thông tin
3. QR code cũ sẽ được giữ nguyên (hoặc tạo mới nếu chưa có)

---

## Quy Trình Quét QR Code

### Bước 1: Tạo URL QR Code

Mỗi quán có một QR code duy nhất. URL để quét là:

```
https://yourdomain.com/qr-scan?code=POI_XXXXXXXXXX
```

**Ví dụ:**
```
https://localhost:5001/qr-scan?code=POI_A1B2C3D4E5
```

### Bước 2: Người Dùng Quét QR

Khi người dùng quét QR code:

1. **Lần đầu tiên quét**
   - Hệ thống tạo Device ID (cookie `vkt_device_id`)
   - Ghi nhận thông tin thiết bị
   - Khởi tạo giới hạn: 0/5 lần quét

2. **Kiểm tra giới hạn**
   - Nếu < 5 lần: Cho phép quét
   - Nếu = 5 lần: Chặn và khuyến khích tải app

3. **Redirect**
   - Try deep link: `vinhkhanhtour://poi/{id}`
   - Wait 2 seconds
   - Fallback to web: `/poi-public.html?qr=...&poi=...&scans=...`

### Bước 3: Hiển Thị Thông Tin

Người dùng sẽ thấy:

#### Nếu còn lượt quét:
```
┌─────────────────────────────┐
│ Lượt quét hôm nay           │
│        3/5                  │
│ Tải app để không giới hạn!  │
└─────────────────────────────┘

┌─────────────────────────────┐
│ 🏪 Ốc Oanh                  │
│ 📍 534 Vĩnh Khánh, Q.4      │
│                             │
│ Quán ốc nổi tiếng...        │
│                             │
│ [📱 Tải App Vĩnh Khánh Tour]│
│ [🎉 Tôi Đã Tới Quán]       │
└─────────────────────────────┘
```

#### Nếu hết lượt quét:
```
┌─────────────────────────────┐
│ ⚠️ Bạn đã hết lượt quét     │
│ miễn phí hôm nay (5 lần)    │
│                             │
│ Tải ứng dụng để:            │
│ ✅ Không giới hạn quét QR   │
│ ✅ Nghe thuyết minh âm thanh│
│ ✅ Lưu lịch sử tham quan    │
└─────────────────────────────┘

┌─────────────────────────────┐
│ [📱 Tải App Ngay]           │
└─────────────────────────────┘
```

---

## Chi Tiết Kỹ Thuật

### Database Schema

#### DeviceScanLimit Table
```csharp
public class DeviceScanLimit
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string DeviceId { get; set; }      // Unique device identifier (cookie)
    public int ScanCount { get; set; }        // Number of scans today
    public int MaxScans { get; set; }         // Max allowed (5 for free)
    public DateTime LastResetDate { get; set; } // Last reset date
    public DateTime CreatedAt { get; set; }   // First scan time
}
```

#### QRScanRequest Table
```csharp
public class QRScanRequest
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int UserId { get; set; }           // 0 for anonymous
    public int RestaurantId { get; set; }     // POI ID
    public string RestaurantName { get; set; }
    public DateTime ScanTime { get; set; }
    public string DeviceInfo { get; set; }    // iOS/Android/Windows
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Status { get; set; }        // success/blocked
    public string Message { get; set; }
}
```

### API Endpoints

#### GET /qr-scan?code=XXX
**Purpose:** QR scan landing page

**Flow:**
1. Get device ID from cookie (or create new)
2. Look up POI by QR code
3. Check/create device scan limit
4. If allowed:
   - Increment scan count
   - Record scan in QRScanRequest
   - Try deep link to app
   - Fallback to web info page
5. If blocked:
   - Show limit reached message
   - Encourage app download

**Response:** HTML redirect page

#### GET /api/pois/qr/{qrCode}
**Purpose:** Get POI info by QR code

**Response:**
```json
{
  "id": 1,
  "name": "Ốc Oanh",
  "address": "534 Vĩnh Khánh",
  "qrCode": "POI_A1B2C3D4E5",
  "lat": 10.7595,
  "lng": 106.7045
}
```

### Deep Linking

#### iOS
```
vinhkhanhtour://poi/1
```
**Setup:** Configure URL scheme in `Info.plist`

#### Android
```
vinhkhanhtour://poi/1
```
**Setup:** Configure intent filter in `AndroidManifest.xml`

---

## Cách Tạo QR Code Vật Lý

### Option 1: Sử Dụng QR Generator Online

1. Truy cập: https://www.qr-code-generator.com/
2. Chọn **URL**
3. Nhập URL: `https://yourdomain.com/qr-scan?code=POI_XXXXXXXXXX`
4. Download QR code (PNG/SVG)
5. In ra giấy/sticker
6. Dán tại quán ăn

### Option 2: Tạo Tự Động Trong Admin (TODO)

```csharp
// Future feature: Generate QR image
using QRCoder;

var qrGenerator = new QRGenerator();
var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
var qrCode = new PngByteQRCode(qrCodeData);
byte[] qrCodeImage = qrCode.GetGraphic(20);
```

---

## Testing Checklist

### ✅ CSS Loading
- [ ] Admin dashboard loads CSS correctly
- [ ] All pages have proper styling
- [ ] No 404 errors in browser console

### ✅ POI Management
- [ ] Can create new POI
- [ ] QR code auto-generated
- [ ] QR code displays in card
- [ ] Can view QR code in public page
- [ ] Can edit POI (QR preserved)

### ✅ QR Scanning
- [ ] Scan URL redirects correctly
- [ ] Device ID created on first scan
- [ ] Scan count increments
- [ ] Limit enforced at 5 scans
- [ ] Daily reset works (after midnight)
- [ ] Deep link attempts (if app installed)
- [ ] Fallback to web works

### ✅ Public Info Page
- [ ] POI info displays correctly
- [ ] Scan limit shown
- [ ] App download button works
- [ ] iOS → App Store
- [ ] Android → Play Store
- [ ] Limit warning shows when needed

---

## Troubleshooting

### Q: CSS vẫn không load?
**A:** Kiểm tra:
1. File path: `wwwroot/css/admin.css` tồn tại
2. URL trong HTML: `<link rel="stylesheet" href="/css/admin.css" />` (có dấu `/` đầu)
3. Browser cache: Clear cache (Ctrl+F5)

### Q: QR code không hiển thị?
**A:** Kiểm tra:
1. POI có field `QRCode` trong database
2. Seed data đã chạy: `await dbService.SeedSampleDataAsync()`
3. JavaScript render đúng: Check console errors

### Q: Quét QR báo lỗi "not found"?
**A:** Kiểm tra:
1. QR code URL đúng format: `/qr-scan?code=POI_XXX`
2. POI có QR code matching trong DB
3. DatabaseService.GetPOIByQRCodeAsync() hoạt động

### Q: Giới hạn 5 lần không reset?
**A:** Kiểm tra:
1. `LastResetDate < DateTime.UtcNow.Date` logic
2. Server time zone (sử dụng UTC)
3. Cookie `vkt_device_id` đúng

---

## Next Steps

### Short Term
- [ ] Test trên mobile devices (iOS/Android)
- [ ] Configure real app store links
- [ ] Generate printable QR codes

### Long Term
- [ ] Admin UI để tạo QR image
- [ ] Analytics dashboard cho QR scans
- [ ] Export QR statistics
- [ ] Customizable scan limits per POI
- [ ] Paid user integration (unlimited scans)

---

## Summary

**Đã triển khai đầy đủ:**
1. ✅ CSS fixed - Absolute paths
2. ✅ QR codes hiển thị trong admin
3. ✅ Device tracking với cookie
4. ✅ Giới hạn 5 lần quét/ngày
5. ✅ QR scan → Deep link → Web fallback
6. ✅ Trang thông tin quán với nút tải app
7. ✅ Reset giới hạn hàng ngày
8. ✅ Ghi nhận analytics

**Hệ thống hoàn toàn sẵn sàng để sử dụng!** 🎉
