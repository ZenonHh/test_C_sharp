# 🐛 Troubleshooting Guide

## Common Issues & Solutions

### **1. Insufficient Storage (ADB0060)**

**Error**: "There is not enough storage space on the device"

**Solutions**:

**Option A: Wipe Emulator (Recommended)**
```powershell
# Tools > Android > Android Device Manager
# Right-click emulator > Wipe Data
# Restart emulator
```

**Option B: Clear Cache**
```powershell
adb shell pm clear com.android.chrome
adb shell pm clear com.android.vending
adb shell pm clear com.android.systemui
```

**Option C: Check Storage**
```powershell
adb shell df -h /data
# If >95% full, wipe emulator
```

---

### **2. Device Not Found**

**Error**: `error: no devices found`

**Solutions**:

```powershell
# 1. Check devices
adb devices -l

# 2. Restart ADB
adb kill-server
adb start-server

# 3. Check again
adb devices -l
```

**If physical device**:
- Reconnect USB cable
- Enable USB Debugging: Settings > Developer Options > USB Debugging
- Trust computer when prompted

---

### **3. Previous Version Error**

**Error**: ".NET Android does not support running the previous version"

**Solution**:
```powershell
adb uninstall com.companyname.doancsharp_clean
dotnet build DoAnCSharp.csproj -f net8.0-android -t Install
```

---

### **4. Build Failed**

**Error**: "Build exited with code 1"

**Solutions**:

```powershell
# Full clean
dotnet clean DoAnCSharp.csproj -f net8.0-android
Remove-Item -Recurse -Force obj, bin

# Restore and rebuild
dotnet restore
dotnet build DoAnCSharp.csproj -f net8.0-android
```

---

### **5. Offline/Version Mismatch**

**Error**: Device shows "offline" in adb

**Solutions**:

```powershell
# Restart ADB
adb kill-server
adb start-server

# Reconnect device
adb devices

# If still offline, restart emulator or remove/reconnect USB
```

---

### **6. Gradle Build Error**

**Error**: Gradle build failures

**Solutions**:

```powershell
# Clear Gradle cache
Remove-Item -Recurse -Force "$env:USERPROFILE\.gradle\caches"

# Rebuild
dotnet build DoAnCSharp.csproj -f net8.0-android
```

---

### **7. Emulator Won't Start**

**Error**: Emulator takes forever or won't start

**Solutions**:

```powershell
# Kill emulator
adb emu kill

# Start fresh
# Tools > Android > Android Emulator > Select device > Start
```

---

### **8. App Crashes on Startup**

**Error**: App installed but crashes immediately

**Solutions**:

```powershell
# View logs
adb logcat -s "*" | findstr "crash"

# or for more detail
adb logcat
```

Common causes:
- Missing permissions
- Database not initialized
- Service not registered in DI

---

### **9. Permissions Not Granted**

App needs these permissions:
- ✅ Camera (for QR scanning)
- ✅ Location (for map)
- ✅ Internet (for API)

If denied, go to:
- Settings > Apps > VinhKhanhFoodTour > Permissions > Enable all

---

### **10. API Connection Issues**

**Error**: API calls fail, timeout

**Solutions**:

```powershell
# Check API is running
# Start AdminWeb API first:
cd DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb
dotnet run

# Check emulator can reach it
adb shell ping 10.0.2.2
```

**Note**: 
- Use `10.0.2.2` on emulator (not `localhost`)
- Use actual IP on physical device

---

## Debug Commands

### View Real-time Logs
```powershell
adb logcat
```

### Filter Logs
```powershell
adb logcat -s "tag_name"
```

### Check Package Info
```powershell
adb shell pm list packages | Select-String "doancsharp"
```

### View App Permissions
```powershell
adb shell pm dump com.companyname.doancsharp_clean | Select-String "permission"
```

---

## Quick Checklist

- [ ] Android SDK installed
- [ ] MAUI workload installed
- [ ] Emulator running or device connected
- [ ] USB Debugging enabled (if device)
- [ ] Sufficient storage (>500MB)
- [ ] Build successful
- [ ] No error logs

---

## Still Having Issues?

1. Check build output: `dotnet build ... -v diag`
2. Review logs: `adb logcat`
3. Verify device: `adb devices -l`
4. Clean everything: `dotnet clean && Remove-Item obj,bin -r`

Good luck! 🚀
