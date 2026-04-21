# 🚀 CÁCH CHẠY ADMIN WEB - ĐƠN GIẢN NHẤT

## ❌ LỖI BẠN ĐANG GẶP:

```
cd : Cannot find path 'C:\Users\LENOVO\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb' because it does not exist.
```

**Nguyên nhân:** Bạn đang chạy command từ **thư mục sai**.

---

## ✅ GIẢI PHÁP:

### **Cách 1: Chạy từ workspace root (KHUYẾN NGHỊ)**

```powershell
# Bước 1: Di chuyển đến thư mục root
cd C:\Users\LENOVO\source\repos\do_an_C_sharp

# Bước 2: Chạy script universal (tự động tìm path)
.\start-admin.ps1
```

**Script `start-admin.ps1` sẽ:**
- ✅ Tự động tìm project path
- ✅ Xóa database cũ
- ✅ Build và chạy server
- ✅ Hoạt động từ bất kỳ thư mục nào

---

### **Cách 2: Chạy script gốc (nếu đã ở đúng thư mục)**

```powershell
# Đảm bảo bạn đang ở: C:\Users\LENOVO\source\repos\do_an_C_sharp
cd C:\Users\LENOVO\source\repos\do_an_C_sharp

# Chạy script
.\DoAnCSharp.AdminWeb\run-admin.ps1
```

---

### **Cách 3: Chạy thủ công (nếu scripts không hoạt động)**

```powershell
# 1. Di chuyển đến project folder
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb

# 2. Build
dotnet build --configuration Release

# 3. Run
dotnet run --configuration Release
```

Sau đó mở: **http://localhost:5000**

---

## 🔍 KIỂM TRA ĐƯỜNG DẪN:

### Kiểm tra bạn đang ở đâu:
```powershell
Get-Location
```

**Kết quả mong đợi:**
```
Path
----
C:\Users\LENOVO\source\repos\do_an_C_sharp
```

### Kiểm tra project có tồn tại không:
```powershell
Test-Path "DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb"
```

**Kết quả mong đợi:** `True`

### Xem cấu trúc thư mục:
```powershell
Get-ChildItem "DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb" | Select-Object Name
```

**Kết quả mong đợi:**
```
Name
----
Controllers
Models
Properties
Services
wwwroot
appsettings.json
DoAnCSharp.AdminWeb.csproj
Program.cs
```

---

## ⚠️ TROUBLESHOOTING:

### **Lỗi: "Cannot find path..."**

**Kiểm tra:**
```powershell
# Hiển thị vị trí hiện tại
Write-Host "Current location: $(Get-Location)"

# Hiển thị workspace root
Write-Host "Workspace root: C:\Users\LENOVO\source\repos\do_an_C_sharp"

# Kiểm tra project tồn tại
if (Test-Path "C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb") {
    Write-Host "✅ Project exists" -ForegroundColor Green
} else {
    Write-Host "❌ Project NOT found" -ForegroundColor Red
}
```

**Giải pháp:**
- Đảm bảo bạn clone repository đúng chỗ
- Đảm bảo folder `DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb` tồn tại

---

### **Lỗi: "Port 5000 already in use"**

```powershell
# Kill tất cả dotnet processes
Get-Process -Name dotnet | Stop-Process -Force
```

---

### **Lỗi: "Build failed"**

```powershell
# Clean build artifacts
cd C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet clean
Remove-Item bin,obj -Recurse -Force -ErrorAction SilentlyContinue

# Build lại
dotnet build --configuration Release
```

---

## 📋 CHECKLIST TRƯỚC KHI CHẠY:

- [ ] Đã mở PowerShell/Terminal
- [ ] Đã `cd` đến `C:\Users\LENOVO\source\repos\do_an_C_sharp`
- [ ] Đã verify project tồn tại với `Test-Path "DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb"`
- [ ] Không có dotnet process nào đang chạy
- [ ] Port 5000 và 5001 không bị chiếm

---

## 🎯 QÚICK START - 3 BƯỚC:

```powershell
# 1. Di chuyển đến workspace root
cd C:\Users\LENOVO\source\repos\do_an_C_sharp

# 2. Chạy script universal
.\start-admin.ps1

# 3. Mở browser và nhấn Ctrl+F5
# URL: http://localhost:5000
```

**XONG!** 🎉

---

## 📞 NẾU VẪN LỖI:

Copy và chạy script này để thu thập thông tin:

```powershell
@"
=== ADMIN WEB DEBUG INFO ===
Current Location: $(Get-Location)
Workspace Root: C:\Users\LENOVO\source\repos\do_an_C_sharp

Project Exists: $(Test-Path 'C:\Users\LENOVO\source\repos\do_an_C_sharp\DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb')

Dotnet Version:
"@ | Out-File debug.txt

dotnet --version | Out-File debug.txt -Append

@"

Files in workspace root:
"@ | Out-File debug.txt -Append

Get-ChildItem C:\Users\LENOVO\source\repos\do_an_C_sharp | Select-Object Name | Out-File debug.txt -Append

Write-Host "Debug info saved to debug.txt" -ForegroundColor Green
notepad debug.txt
```

Sau đó gửi file `debug.txt` để tôi xem.

---

**TÓM LẠI:** Đơn giản nhất là chạy `.\start-admin.ps1` từ thư mục `C:\Users\LENOVO\source\repos\do_an_C_sharp`
