# Hướng Dẫn Debug Trang Admin Web

## 🔍 Khi Trang Bị Đơ/Không Phản Hồi

### Bước 1: Mở Browser Developer Tools
1. Nhấn `F12` hoặc `Ctrl+Shift+I` (Windows/Linux) hoặc `Cmd+Option+I` (Mac)
2. Chọn tab **Console**
3. Refresh trang (`F5`) và xem có lỗi gì không

### Bước 2: Kiểm Tra Console Errors

#### ❌ Lỗi Thường Gặp:

**1. Không load được CSS/JS:**
```
GET http://localhost:5000/css/admin.css 404 (Not Found)
GET http://localhost:5000/js/devices.js 404 (Not Found)
```
**Giải pháp:**
- Kiểm tra file có tồn tại trong `wwwroot/css/` và `wwwroot/js/`
- Restart project: `Ctrl+C` rồi `dotnet run`

**2. API không tìm thấy:**
```
GET http://localhost:5000/api/users 404 (Not Found)
```
**Giải pháp:**
- Kiểm tra Controller có route đúng không
- Kiểm tra `Program.cs` đã map controllers: `app.MapControllers();`

**3. JavaScript errors:**
```
Uncaught TypeError: Cannot read property 'classList' of null
```
**Giải pháp:**
- Element ID không tồn tại trong HTML
- Kiểm tra `document.getElementById("...")` có đúng ID không

**4. CORS errors:**
```
Access to fetch at '...' from origin '...' has been blocked by CORS policy
```
**Giải pháp:**
- Kiểm tra `Program.cs` đã enable CORS chưa

### Bước 3: Kiểm Tra Network Tab

1. Mở tab **Network** trong DevTools
2. Refresh trang
3. Xem các requests:
   - ✅ Status 200 = OK
   - ❌ Status 404 = Không tìm thấy
   - ❌ Status 500 = Lỗi server

#### Kiểm Tra Chi Tiết Request:
1. Click vào request bị lỗi
2. Xem **Response** để biết lỗi cụ thể
3. Xem **Headers** để biết request có đúng không

### Bước 4: Kiểm Tra HTML Structure

Sử dụng tab **Elements** trong DevTools:

**Cấu trúc đúng:**
```html
<header>
  <div class="tabs">
    <!-- CHỈ buttons ở đây -->
    <button class="tab-btn">...</button>
  </div>
</header>

<div class="content">
  <!-- TẤT CẢ tab-content divs ở đây -->
  <div id="dashboard" class="tab-content active">...</div>
  <div id="pois" class="tab-content">...</div>
  <div id="users" class="tab-content">...</div>
  ...
</div>
```

**❌ SAI - div lồng sai:**
```html
<div class="tabs">
  <button>...</button>
  <!-- SAI - tab-content KHÔNG ĐƯỢC nằm ở đây -->
  <div id="users" class="tab-content">...</div>
</div>
```

## 🧪 Test Các Chức Năng

### Test 1: Tab Switching
```javascript
// Mở Console và chạy:
switchTab('users');
switchTab('pois');
switchTab('devices');
```
Nếu không có lỗi → Tab switching hoạt động

### Test 2: API Calls
```javascript
// Mở Console và chạy:
fetch('/api/users')
  .then(r => r.json())
  .then(data => console.log(data));
```
Nếu hiển thị data → API hoạt động

### Test 3: Check Functions
```javascript
// Mở Console và chạy:
console.log(typeof loadUsers);        // Should be "function"
console.log(typeof loadPOIs);         // Should be "function"
console.log(typeof loadDevices);      // Should be "function" (from devices.js)
```
Nếu tất cả đều là "function" → JavaScript đã load đầy đủ

## 🔧 Các Lỗi Phổ Biến và Cách Fix

### Lỗi 1: Placeholder Functions Chưa Implement
**Triệu chứng:** Click vào tab, không hiển thị gì, không có lỗi

**Kiểm tra:**
```javascript
async function loadUsers() { /* Placeholder */ }  // ❌ SAI
```

**Fix:**
```javascript
async function loadUsers() {
  const container = document.getElementById("userList");
  try {
    const response = await fetch(`${API_BASE}/users`);
    const users = await response.json();
    // Render data...
  } catch (error) {
    console.error(error);
  }
}
```

### Lỗi 2: Element ID Không Tồn Tại
**Triệu chứng:** Console error: `Cannot read property 'classList' of null`

**Kiểm tra HTML:**
```html
<!-- HTML phải có element với ID này -->
<div id="userList"></div>
```

**Kiểm tra JavaScript:**
```javascript
// Đảm bảo ID khớp
document.getElementById("userList");  // phải match với HTML
```

### Lỗi 3: CSS Không Load
**Triệu chứng:** Trang không có màu sắc, layout lỗi

**Kiểm tra:**
```html
<head>
  <!-- Đường dẫn phải đúng -->
  <link rel="stylesheet" href="css/admin.css" />
  <link rel="stylesheet" href="css/devices.css" />
</head>
```

**File structure:**
```
wwwroot/
├── css/
│   ├── admin.css     ← File phải tồn tại
│   └── devices.css   ← File phải tồn tại
└── index.html
```

### Lỗi 4: API Endpoint Không Đúng
**Triệu chứng:** 404 Not Found khi gọi API

**Kiểm tra Controller:**
```csharp
[ApiController]
[Route("api/[controller]")]  // Route: /api/users
public class UsersController : ControllerBase
{
    [HttpGet]  // GET /api/users
    public async Task<ActionResult<List<User>>> GetUsers() { }
}
```

**Kiểm tra JavaScript:**
```javascript
const API_BASE = "/api";  // Không có trailing slash
fetch(`${API_BASE}/users`);  // = /api/users ✅
```

## 📝 Checklist Debug

Khi trang bị đơ, check theo thứ tự:

- [ ] **Console có lỗi gì không?**
  - Nếu có: Fix lỗi đó trước
  
- [ ] **CSS/JS files có load được không?**
  - Check Network tab
  - Kiểm tra file paths
  
- [ ] **HTML structure có đúng không?**
  - Tab-content divs nằm trong `.content`
  - Không có divs lồng sai
  
- [ ] **JavaScript functions có được implement không?**
  - Không còn placeholder functions `{ /* Placeholder */ }`
  - Tất cả functions đều có code xử lý
  
- [ ] **API endpoints có hoạt động không?**
  - Test bằng browser: `http://localhost:5000/api/users`
  - Hoặc dùng Postman/curl
  
- [ ] **Event handlers có được attach đúng không?**
  - `onclick="switchTab('...')"` có function tương ứng
  - Event listeners được add sau khi DOM ready

## 🚀 Quick Fixes

### Fix Nhanh 1: Clear Cache
```
Ctrl+Shift+Delete → Clear browsing data → Cached images and files
```
Hoặc hard reload: `Ctrl+Shift+R` (Windows/Linux) hoặc `Cmd+Shift+R` (Mac)

### Fix Nhanh 2: Restart Project
```powershell
# Trong terminal:
Ctrl+C  # Stop project
dotnet run --project DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb
```

### Fix Nhanh 3: Rebuild Project
```powershell
dotnet clean
dotnet build
dotnet run --project DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb
```

## 📊 Monitoring Tools

### Chrome DevTools Performance
1. Tab **Performance**
2. Click Record (●)
3. Click vào tab bị đơ
4. Stop recording
5. Xem có function nào bị stuck không

### Chrome DevTools Coverage
1. `Ctrl+Shift+P` → "Show Coverage"
2. Click Reload
3. Xem CSS/JS nào không được sử dụng

## 🔗 Tài Liệu Tham Khảo

- [FIX_FROZEN_PAGE.md](./FIX_FROZEN_PAGE.md) - Chi tiết fix lỗi trang đơ
- [USER_GUIDE_COMPLETE.md](./USER_GUIDE_COMPLETE.md) - Hướng dẫn sử dụng
- [IMPLEMENTATION_FINAL.md](./IMPLEMENTATION_FINAL.md) - Chi tiết implementation

---
**Lưu ý:** Luôn check Console trước tiên - 90% lỗi sẽ được hiển thị ở đó!
