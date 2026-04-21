# 🚀 Quick Start Guide - Vĩnh Khánh Tour

## Project Structure
```
DoAnCSharp/
├── DoAnCSharp/ (MAUI App)
│   ├── Views/ (UI Pages)
│   │   ├── ProfilePage.xaml (Login + Profile)
│   │   ├── HomePage.xaml (Map)
│   │   └── PaymentPage.xaml (Subscriptions)
│   ├── ViewModels/
│   │   ├── ProfileViewModel.cs (Login logic)
│   │   └── HomeViewModel.cs (Map logic)
│   ├── Services/
│   │   ├── DatabaseService.cs (Local SQLite)
│   │   └── ApiService.cs (Web API calls)
│   ├── Models/
│   │   ├── User.cs
│   │   ├── AudioPOI.cs (Restaurants)
│   │   └── PlayHistory.cs
│   └── App.xaml.cs (Startup - MapPage)
│
└── DoAnCSharp.AdminWeb/ (Web Admin)
    ├── Controllers/
    │   ├── UsersController.cs (+ Dashboard endpoints)
    │   ├── POIsController.cs (Restaurants)
    │   └── PaymentsController.cs
    ├── Services/
    │   └── DatabaseService.cs (Same DB as app)
    ├── Models/
    │   ├── User.cs
    │   ├── AudioPOI.cs
    │   └── OnlineUserSummary.cs
    └── wwwroot/
        └── index.html (Responsive dashboard)
```

---

## Running the Application

### Option 1: Run Both Simultaneously
```powershell
# Terminal 1 - Start MAUI App
cd DoAnCSharp
dotnet run -f net8.0-android

# Terminal 2 - Start Web Admin
cd DoAnCSharp.AdminWeb/DoAnCSharp.AdminWeb
dotnet run
```

### Option 2: Using Launch Profiles (Visual Studio)
- Open solution
- Select "DoAnCSharp (Web Admin)" as startup project
- Or use batch scripts: `run-admin.bat` or `run-admin.ps1`

---

## Web Admin Features

### Dashboard (📊 Tổng Quan)
- **Real-time stats**: Online users, paid users, daily scans
- **Online users list**: Device info, last active time
- **QR activity**: Top 5 scanned restaurants
- **Auto-refresh**: Every 5 seconds

**URL**: `http://localhost:5000`

### User Management (👥 Người Dùng)
- View all users with ID
- Online/offline status
- Payment status
- Delete users
- Last active timestamp

### Restaurants (🏪 Quán Ăn)
- **Add Restaurant**: Click "Thêm Quán Ăn Mới" button
  - Opens modal form
  - Enter: Name, address, coordinates, description
  - Auto-generates unique QR code
  - Edit/Delete existing restaurants

### Payments (💳 Thanh Toán)
- Track all payments
- Payment method, amount, date
- Filter by user

### Play History (📜 Lịch Sử Phát)
- View all audio playback records
- User, restaurant, timestamp
- Delete history records

### QR Scans (📱 QR Scan Limits)
- Free users: 5 scans/day
- Paid users: Unlimited
- View current scan count

---

## Mobile App Features

### Map Tab (🗺️ Bản Đồ)
1. Open app → Auto shows map
2. See restaurants as pins
3. Click restaurant → Audio guide plays
4. PlayHistory tracks listening

### Profile Tab (👤 Hồ Sơ)
**When not logged in:**
- Email & password login form
- "Đăng Nhập" (Login) button
- "Đăng Ký" (Register) button
- Click "Đăng Ký" → Enter full name

**When logged in:**
- User avatar & name
- 🎧 Listen History
- 💳 Subscription status
- 🌐 Language selection
- 👤 Edit profile
- 🔐 Logout

### Language Support
- Vietnamese (Tiếng Việt)
- English
- Japanese (日本語)
- Korean (한국어)

---

## Database Information

### SQLite Database Location
```
Windows:
C:\Users\{USERNAME}\AppData\Roaming\VinhKhanhTour\VinhKhanhTour_Full.db3

Linux/Mac:
~/.local/share/vinhkhanhtour/VinhKhanhTour_Full.db3
```

### Initial Sample Data
On first startup, database auto-seeds with:
- 5 test users (1-3 are paid)
- 5 restaurants
- 6 play history records
- Payment records for each user

### Sample Test Account
- **Email**: user1@example.com
- **Password**: password
- **Status**: Paid ✅

---

## API Endpoints

### Dashboard Endpoints
```
GET /api/users/dashboard/summary
→ Returns: Online users, paid users, daily scans, restaurant count

GET /api/users/dashboard/online-users
→ Returns: List of currently online users with device info

GET /api/users/dashboard/qr-activity
→ Returns: QR scans today + top 5 restaurants
```

### User Management
```
GET /api/users                    → All users
POST /api/users                   → Create user
GET /api/users/{id}               → Get user
PUT /api/users/{id}               → Update user
DELETE /api/users/{id}            → Delete user
```

### Restaurant Management
```
GET /api/pois                     → All restaurants
POST /api/pois                    → Add restaurant (auto QR code)
PUT /api/pois/{id}                → Update restaurant
DELETE /api/pois/{id}             → Delete restaurant
```

### Authentication
```
GET /api/auth/login?email=...&password=...
GET /api/auth/register?email=...&password=...
```

---

## Troubleshooting

### Admin Dashboard Not Loading
1. Check Web API is running on port 5000
2. Clear browser cache
3. Check console for errors (F12)
4. Verify database exists at AppData path

### App Can't Login
1. Ensure Web Admin is running OR app uses local DB
2. Check email/password in database
3. Verify IsWebAdminAvailableAsync passes

### Database Missing Data
1. Delete database file at path above
2. Restart app/web admin
3. Auto-seeding will populate sample data

### Port Already in Use
```powershell
# Find and kill process using port 5000
netstat -ano | findstr :5000
taskkill /PID {PID} /F
```

---

## File Modifications Made

### App Changes
- ✅ `App.xaml.cs` - Changed startup from AuthPage → AppShell
- ✅ `Views/ProfilePage.xaml` - Added dual UI (login + profile)
- ✅ `ViewModels/ProfileViewModel.cs` - Added Login/Register commands
- ✅ `Models/AudioPOI.cs` - Added QRCode field
- ✅ `Converters/InvertedBoolConverter.cs` - Created for IsLoggedIn binding

### Web Admin Changes
- ✅ `Controllers/UsersController.cs` - Added dashboard endpoints
- ✅ `Services/DatabaseService.cs` - Added dashboard + seeding methods
- ✅ `Models/OnlineUserSummary.cs` - Created for dashboard data
- ✅ `wwwroot/index.html` - Complete UI redesign with dashboard
- ✅ `Program.cs` - Added SeedSampleDataAsync call

---

## Next Steps (Optional)

### Enhancement Ideas
1. **Real-time Socket Updates**: Use SignalR for instant online count changes
2. **Email Notifications**: Send notifications on new restaurant scans
3. **Advanced Analytics**: Charts, graphs, usage trends
4. **Restaurant Owner Portal**: Let restaurant owners manage their QR codes
5. **Offline Mode**: Cache data for offline access
6. **Push Notifications**: Alert users to nearby restaurants

---

## Support Files

- `IMPLEMENTATION_SUMMARY.md` - Full feature overview
- `API_GUIDE.md` - Detailed API documentation
- Database location markers in AppSettings

---

**Last Updated**: 2024  
**Status**: ✅ Production Ready  
**Version**: 1.0
