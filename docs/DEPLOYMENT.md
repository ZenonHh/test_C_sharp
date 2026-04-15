# 🚀 Deployment Guide

## Quick Deploy

### **Method 1: Visual Studio (Fastest)**
1. Open project in VS
2. Select Android Emulator/Device from top toolbar
3. Press `F5` or `Build > Deploy Solution`

### **Method 2: PowerShell Script**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
.\prepare_android_build.ps1
```

### **Method 3: Command Line**
```powershell
# Clean and restore
dotnet clean DoAnCSharp.csproj -f net8.0-android
dotnet restore

# Remove old APK
adb uninstall com.companyname.doancsharp_clean

# Build and install
dotnet build DoAnCSharp.csproj -f net8.0-android -t Install
```

---

## Prerequisites

- ✅ Visual Studio 2022+ or .NET 8 SDK
- ✅ Android SDK (API 21+)
- ✅ MAUI workload: `dotnet workload install maui-android`
- ✅ Android Emulator or USB Device

---

## Step-by-Step Setup

### **1. Install MAUI Workload**
```powershell
dotnet workload install maui-android
```

### **2. Start Android Emulator**
- Windows Start Menu > Android Emulator
- Select device > Click "Start"
- Wait 2-3 minutes for startup

### **3. Deploy**
```powershell
F5 in Visual Studio
```

---

## First-Time Setup

1. Check device:
   ```powershell
   adb devices -l
   ```

2. Enable USB Debugging (if physical device):
   - Settings > Developer Options > USB Debugging

3. Check storage:
   ```powershell
   adb shell df -h /data
   ```

4. Build:
   ```powershell
   dotnet build DoAnCSharp.csproj -f net8.0-android
   ```

---

## APK Details

- **Package Name**: com.companyname.doancsharp_clean
- **Size**: ~150-200 MB
- **Platforms**: Android 5.0+ (API 21+)
- **Permissions**: Camera, Location, Internet

---

## Post-Deployment

App will request:
- ✅ Camera permission (QR scanning)
- ✅ Location permission (Map)
- ✅ Internet permission (API calls)

Grant all permissions for full functionality.

---

## Troubleshooting

**Storage Full?**
```powershell
# Wipe emulator
Tools > Android > Android Device Manager > Wipe Data
```

**Device Not Found?**
```powershell
adb kill-server
adb start-server
adb devices
```

**Previous Version Error?**
```powershell
adb uninstall com.companyname.doancsharp_clean
```

See TROUBLESHOOTING.md for more help.
