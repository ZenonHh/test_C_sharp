# 🍽️ VinhKhanh Food Tour - .NET MAUI App

**A modern food tour guide app with QR code scanning, payment system, and web admin panel.**

---

## 📋 Quick Start

### **Build Status**
```
✅ Clean Build (0 Errors)
✅ MAUI App Ready
✅ Web Admin API Ready
```

### **Run the App Now**

**Option 1: Visual Studio (Fastest)**
```
1. Open project in VS
2. Select Android Emulator/Device  
3. Press F5
```

**Option 2: Command Line**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
dotnet build DoAnCSharp.csproj -f net8.0-android -t Install
```

**Option 3: Auto Script**
```powershell
.\prepare_android_build.ps1
```

---

## 🎯 Project Structure

```
do_an_C_sharp/
├── 📱 MAUI App (Android)
│   ├── Views/               (UI Pages)
│   ├── Services/            (API Clients, Database)
│   ├── Models/              (Data Models)
│   └── MauiProgram.cs       (DI Setup)
│
├── 🌐 Web Admin (ASP.NET Core)
│   ├── Controllers/         (API Endpoints)
│   ├── Models/              (Data Models)
│   ├── Services/            (Business Logic)
│   └── Program.cs           (Startup)
│
├── 💾 Database
│   └── VinhKhanhTour_Full.db3 (SQLite)
│
├── 📚 docs/                 (Documentation)
│   ├── DEPLOYMENT.md
│   ├── TROUBLESHOOTING.md
│   ├── API_GUIDE.md
│   └── CHANGELOG.md
│
└── 🛠️ Scripts
    ├── prepare_android_build.ps1
    └── prepare_android_build.bat
```

---

## ✨ Features

✅ QR Code Scanning  
✅ Payment System (Free/Premium)  
✅ Real-time Map  
✅ Scan Limits (5/day free)  
✅ Web Admin Panel  
✅ User Management  
✅ Payment Tracking  

---

## 🚀 Technology

- **Frontend**: .NET MAUI 8.0 (Android)
- **Backend**: ASP.NET Core 8.0
- **Database**: SQLite
- **QR Scanning**: ZXing.Net.Maui
- **Maps**: Mapsui

---

## 🔧 Latest Updates

✅ Fixed 6 compilation errors  
✅ Clean build (0 errors)  
✅ Production ready  

---

## 📖 Full Documentation

See `docs/` folder for:
- Deployment guide
- Troubleshooting tips
- API documentation
- Changelog

---

## 🚀 Deploy Now

```powershell
F5  # Visual Studio
# or
dotnet build DoAnCSharp.csproj -f net8.0-android -t Install
```

---

## 🐛 Issues?

Check `docs/TROUBLESHOOTING.md` or run:
```powershell
dotnet build DoAnCSharp.csproj -f net8.0-android
```

---

**Let's go! 🎉**
