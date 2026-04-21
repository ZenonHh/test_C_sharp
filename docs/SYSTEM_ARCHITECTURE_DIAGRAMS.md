# 📊 System Architecture & Flow Diagrams

## 🏗️ **Overall System Architecture**

```
┌─────────────────────────────────────────────────────────────────┐
│                         USER DEVICES                            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐          │
│  │   MAUI App   │  │   MAUI App   │  │   MAUI App   │  ...     │
│  │  (iPhone)    │  │  (Android)   │  │  (Tablet)    │          │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘          │
│         │                 │                 │                   │
└─────────┼─────────────────┼─────────────────┼───────────────────┘
          │                 │                 │
          └─────────────────┼─────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │    AdminWeb (ASP.NET Core) API        │
        │    http://localhost:5000/api          │
        │                                       │
        │  ┌─────────────────────────────────┐ │
        │  │    Controllers                  │ │
        │  │  • DevicesController            │ │
        │  │  • UsersController              │ │
        │  │  • POIsController               │ │
        │  │  • PaymentsController           │ │
        │  │  • QRScansController            │ │
        │  │  • (More ready to implement)    │ │
        │  └─────────────────────────────────┘ │
        │                                       │
        │  ┌─────────────────────────────────┐ │
        │  │    DatabaseService              │ │
        │  │  • Query execution              │ │
        │  │  • Data validation              │ │
        │  │  • Transaction management       │ │
        │  └─────────────────────────────────┘ │
        └───────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │      SQLite Database                  │
        │  VinhKhanhTour_Full.db3               │
        │                                       │
        │  Tables (13):                         │
        │  ├── Users                            │
        │  ├── AudioPOI (Restaurants)           │
        │  ├── PlayHistory                      │
        │  ├── UserPayment                      │
        │  ├── QRScanLimit                      │
        │  ├── UserStatus                       │
        │  ├── UserDevice ✨ NEW                │
        │  ├── RestaurantImage ✨ NEW           │
        │  ├── AdminUser ✨ NEW                 │
        │  ├── SystemSetting ✨ NEW             │
        │  ├── AuditLog ✨ NEW                  │
        │  ├── QRCodeSession ✨ NEW             │
        │  └── QRScanRequest ✨ NEW             │
        └───────────────────────────────────────┘
                            ▲
                            │
        ┌───────────────────────────────────────┐
        │   Web Admin Dashboard (HTML/JS)       │
        │   http://localhost:5000               │
        │                                       │
        │  Tabs:                                │
        │  ├── Dashboard                        │
        │  ├── Users                            │
        │  ├── Restaurants                      │
        │  ├── Payments                         │
        │  ├── QR Scans                         │
        │  ├── Devices ✨ NEW                   │
        │  ├── Admin Users ✨ NEW               │
        │  ├── Settings ✨ NEW                  │
        │  ├── Audit Logs ✨ NEW                │
        │  └── Statistics ✨ NEW                │
        └───────────────────────────────────────┘
```

---

## 🔄 **QR Code Scanning Flow**

```
┌──────────────────────┐
│  Restaurant Staff    │
└──────────┬───────────┘
           │
           │ 1. Opens Restaurant Web
           │    (restaurant-qr.html)
           ▼
    ┌──────────────────┐
    │  Restaurant Web  │
    │  (New page)      │
    │                  │
    │ [Generate QR]    │
    └────────┬─────────┘
             │
             │ 2. POST /api/qrcodes/generate/{restaurantId}
             │    Duration: 5 minutes
             ▼
    ┌──────────────────────────┐
    │ QRCodeSession Created    │
    │                          │
    │ • QRCode: ABC123DEF456   │
    │ • Token: uuid-token      │
    │ • ExpiresAt: now+5m      │
    │ • ScanCount: 0           │
    └────────┬─────────────────┘
             │
             │ 3. Display QR Code (25cm x 25cm)
             │    with countdown timer
             ▼
    ┌──────────────────────────┐
    │ 4:59 ← Timer Countdown   │
    │ [QR Code Image]          │
    │ "Ask restaurant staff"   │
    └────────┬─────────────────┘
             │
             │ 4. Tourist scans with MAUI app
             ▼
    ┌──────────────────────────┐
    │ MAUI App (User)          │
    │                          │
    │ [Scan QR Code]           │
    │ Extracts: uuid-token     │
    └────────┬─────────────────┘
             │
             │ 5. POST /api/qrcodes/scan
             │    {
             │      userId: 1,
             │      userName: "Nguyen Van A",
             │      userEmail: "user@example.com",
             │      qrSessionToken: "uuid-token",
             │      deviceInfo: "iPhone 12",
             │      ipAddress: "192.168.1.100"
             │    }
             ▼
    ┌──────────────────────────┐
    │ Backend Verification     │
    │                          │
    │ 1. Verify token exists   │
    │ 2. Check if not expired  │
    │ 3. Log scan request      │
    │ 4. Update online status  │
    │ 5. Increment scan count  │
    └────────┬─────────────────┘
             │
             │ 6. Response: Success
             ▼
    ┌──────────────────────────┐
    │ MAUI App Updates         │
    │                          │
    │ ✓ User online status: UP │
    │ ✓ Device marked online   │
    │ ✓ Can listen to audio    │
    └────────┬─────────────────┘
             │
             │ 7. Restaurant Web shows stats
             ▼
    ┌──────────────────────────┐
    │ Restaurant Web Updates   │
    │                          │
    │ "Today's Scans: 25"      │
    │ "Pending Approvals: 5"   │
    │ "Online Users: 18"       │
    └────────┬─────────────────┘
             │
             │ 8. Auto-generate new QR
             │    (after 5 minutes expire)
             ▼
    ┌──────────────────────────┐
    │ QRCodeSession Refreshed  │
    │                          │
    │ • New QR Code Created    │
    │ • Old session marked:    │
    │   isActive = false       │
    │ • Timer reset to 5:00    │
    └──────────────────────────┘
```

---

## 📊 **Device Management Flow**

```
┌─────────────────────┐
│  User Downloads     │
│  MAUI App           │
└──────────┬──────────┘
           │
           │ First Launch
           ▼
    ┌──────────────────┐
    │ Device Detection │
    │                  │
    │ • Get device ID  │
    │ • Get OS type    │
    │ • Get model      │
    │ • Get app ver    │
    └────────┬─────────┘
             │
             │ POST /api/devices
             │ {
             │   userId: 1,
             │   deviceId: "unique-id",
             │   deviceName: "John's iPhone",
             │   deviceModel: "iPhone12,1",
             │   deviceOS: "iOS",
             │   appVersion: "1.0.0"
             │ }
             ▼
    ┌──────────────────────┐
    │ Device Registered    │
    │                      │
    │ ✓ Stored in DB      │
    │ ✓ Status: online    │
    │ ✓ IP tracked        │
    │ ✓ Location stored   │
    └────────┬─────────────┘
             │
             └─────────────────────────────────────┐
                                                   │
        ┌──────────────────────────────────────────┘
        │
        │ App Open → Update Status
        │ PUT /api/devices/{deviceId}/status
        │ {
        │   isOnline: true,
        │   ipAddress: "192.168.1.100"
        │ }
        ▼
    ┌──────────────────────┐
    │ Device Online        │
    │                      │
    │ ✓ LastOnlineAt: now │
    │ ✓ IP updated        │
    │ ✓ Location updated  │
    └────────┬─────────────┘
             │
             │ App Close → Update Status
             │ PUT /api/devices/{deviceId}/status
             │ {
             │   isOnline: false
             │ }
             ▼
    ┌──────────────────────┐
    │ Device Offline       │
    │                      │
    │ ✓ LastOnlineAt kept │
    │ ✓ Still in DB       │
    │ ✓ Can reactivate    │
    └──────────────────────┘
```

---

## 👨‍💼 **Admin Dashboard Flow**

```
┌─────────────────────┐
│  Admin User         │
└──────────┬──────────┘
           │
           │ Open Web Admin
           │ http://localhost:5000
           ▼
    ┌──────────────────┐
    │  Login Page      │
    │                  │
    │ Username: _____  │
    │ Password: _____  │
    │ [Login]          │
    └────────┬─────────┘
             │
             │ POST /api/admin/login
             │ Verify credentials
             ▼
    ┌──────────────────────────┐
    │ Admin Authenticated      │
    │                          │
    │ • Login logged           │
    │ • IP tracked             │
    │ • User agent saved       │
    │ • LastLoginAt updated    │
    └────────┬─────────────────┘
             │
             │ Access Dashboard
             ▼
    ┌──────────────────────────────────────┐
    │         Admin Dashboard              │
    │                                      │
    │ ┌────────────────────────────────┐  │
    │ │ Tabs:                          │  │
    │ │ • Dashboard                    │  │
    │ │ • Users                        │  │
    │ │ • Restaurants                  │  │
    │ │ • Payments                     │  │
    │ │ • QR Scans                     │  │
    │ │ • Devices ✨                   │  │
    │ │ • Admin Users ✨               │  │
    │ │ • Settings ✨                  │  │
    │ │ • Audit Logs ✨                │  │
    │ │ • Statistics ✨                │  │
    │ └────────────────────────────────┘  │
    │                                      │
    │ ┌────────────────────────────────┐  │
    │ │ Key Metrics:                   │  │
    │ │ • Online Users: 15             │  │
    │ │ • Today's Scans: 235           │  │
    │ │ • Devices: 42 (28 online)      │  │
    │ │ • Revenue: 50M VND             │  │
    │ │ • Paid Users: 45               │  │
    │ └────────────────────────────────┘  │
    └────────┬───────────────────────────┘
             │
             │ Click on different tabs
             ▼
    ┌──────────────────────────┐
    │ View Device Management   │
    │                          │
    │ Devices List:            │
    │ ┌────────────────────┐   │
    │ │ Device 1 - Online  │   │
    │ │ iPhone 12, iOS 17  │   │
    │ │ IP: 192.168.1.100  │   │
    │ │ Last: 2 mins ago   │   │
    │ └────────────────────┘   │
    │ ┌────────────────────┐   │
    │ │ Device 2 - Offline │   │
    │ │ Android, 14.0      │   │
    │ │ IP: 192.168.1.101  │   │
    │ │ Last: 1 hour ago   │   │
    │ └────────────────────┘   │
    └──────────────────────────┘
             │
             │ View Statistics
             ▼
    ┌──────────────────────────┐
    │ Weekly Statistics        │
    │                          │
    │ Scans per day:           │
    │ ├── Sun: 150 (███████)   │
    │ ├── Mon: 165 (████████)  │
    │ ├── Tue: 140 (██████)    │
    │ ├── Wed: 155 (███████)   │
    │ ├── Thu: 170 (████████)  │
    │ ├── Fri: 145 (██████)    │
    │ └── Sat: 160 (███████)   │
    │                          │
    │ Total: 1085 scans        │
    └──────────────────────────┘
             │
             │ View Audit Logs
             ▼
    ┌──────────────────────────┐
    │ Activity Tracking        │
    │                          │
    │ 14:30 - User 5 Logged In │
    │ 14:25 - Device 3 Online  │
    │ 14:20 - QR Code Scanned  │
    │ 14:15 - User 3 Registered│
    │ 14:10 - Settings Updated │
    │ 14:05 - Admin Login      │
    └──────────────────────────┘
             │
             │ Logout
             │ POST /api/admin/{id}/logout
             ▼
    ┌──────────────────────────┐
    │ Logout Complete          │
    │                          │
    │ ✓ Logged out            │
    │ ✓ Session ended         │
    │ ✓ Action logged         │
    └──────────────────────────┘
```

---

## 📈 **Data Flow - Real-time Updates**

```
User Scans QR
     ↓
API Receives Scan
     ↓
┌────────────────────────────┐
│ Database Updates:          │
│ 1. QRScanRequest inserted │
│ 2. UserStatus updated     │
│ 3. QRCodeSession updated  │
│ 4. AuditLog inserted      │
└────────┬───────────────────┘
         ↓
┌────────────────────────────┐
│ Real-time Effects:         │
│ ✓ Online users count +1   │
│ ✓ Scans today counter +1  │
│ ✓ Device marked online    │
│ ✓ Action logged           │
└────────┬───────────────────┘
         ↓
┌────────────────────────────┐
│ Web Admin Sees:            │
│ • Device now online        │
│ • Scan count increased     │
│ • User appears in pending  │
│ • Audit log updated        │
└────────────────────────────┘
```

---

## 🔐 **Authentication & Authorization**

```
┌──────────────────────────┐
│ Roles in System          │
│                          │
│ ┌────────────────────┐  │
│ │ ADMIN              │  │
│ │ • Full access      │  │
│ │ • Create users     │  │
│ │ • Manage admins    │  │
│ │ • Change settings  │  │
│ │ • View all logs    │  │
│ └────────────────────┘  │
│                          │
│ ┌────────────────────┐  │
│ │ MANAGER            │  │
│ │ • View data        │  │
│ │ • Manage users     │  │
│ │ • See statistics   │  │
│ │ • Cannot delete    │  │
│ └────────────────────┘  │
│                          │
│ ┌────────────────────┐  │
│ │ VIEWER             │  │
│ │ • View only        │  │
│ │ • See reports      │  │
│ │ • No modifications │  │
│ └────────────────────┘  │
└──────────────────────────┘
```

---

## 🗄️ **Database Relationships**

```
User
  │ 1─────────N
  ├─→ UserDevice (devices)
  │ 1─────────N
  ├─→ PlayHistory (listening)
  │ 1─────────N
  └─→ UserPayment (payments)

AudioPOI (Restaurant)
  │ 1─────────N
  ├─→ RestaurantImage (images)
  │ 1─────────N
  ├─→ QRCodeSession (QR codes)
  │ 1─────────N
  └─→ PlayHistory (scans)

QRCodeSession
  │ 1─────────N
  └─→ QRScanRequest (scans)

AdminUser
  │ 1─────────N
  ├─→ AuditLog (actions)
  │ 1─────────N
  └─→ SystemSetting (config)

SystemSetting
  └─ Global configuration
```

---

## 📊 **Statistics Aggregation Pipeline**

```
Raw Data
├── Users table (users)
├── UserDevice table (devices)
├── UserPayment table (payments)
├── PlayHistory table (scans)
├── AudioPOI table (restaurants)
└── RestaurantImage table (images)
        ↓
DashboardStatistics (Aggregates)
├── User Statistics
│   ├── totalUsers
│   ├── paidUsers
│   ├── activeUsers
│   └── newUsersThisWeek
├── Device Statistics
│   ├── totalDevices
│   ├── onlineDevices
│   ├── androidDevices
│   └── iosDevices
├── Restaurant Statistics
│   ├── totalRestaurants
│   ├── restaurantsWithImages
│   ├── restaurantsWithQR
│   └── newRestaurantsThisWeek
├── Payment Statistics
│   ├── totalRevenue (VND)
│   ├── totalRevenue (USD)
│   ├── transactions
│   └── averageTransaction
├── QR Statistics
│   ├── totalScans
│   ├── scansToday
│   ├── scansThisMonth
│   ├── uniqueUsersScanned
│   └── topRestaurants
└── Top Performers
    ├── bestRestaurant
    ├── topUsers
    └── mostActiveDevices
        ↓
Web Admin Dashboard
├── Key Metrics Card
├── Charts & Graphs
├── Tables & Lists
└── Detailed Reports
```

---

## ⚡ **Performance Optimization**

```
✅ Async/Await
   └── Non-blocking database calls

✅ Indexing
   ├── UserId index on UserDevice
   ├── RestaurantId index on QRCodeSession
   └── CreatedAt index on AuditLog

✅ Lazy Loading
   ├── Load related data only when needed
   └── Reduce initial load time

✅ Caching Potential
   ├── Settings (change infrequently)
   ├── Top restaurants (refresh hourly)
   └── Statistics (refresh daily)

✅ Batch Operations
   ├── Seed data in bulk
   └── Export data efficiently
```

---

**All systems integrated and ready for deployment!** 🚀
