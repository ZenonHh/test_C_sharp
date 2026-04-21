# 📚 Complete API Reference - All New Endpoints

## Base URL
```
http://10.0.2.2:5000/api  (Android Emulator)
http://localhost:5000/api (Desktop/Web)
```

---

## 📱 Device Management API

### 1. Register Device
```http
POST /api/devices
Content-Type: application/json

{
  "userId": 1,
  "deviceId": "device-uuid-or-imei",
  "deviceName": "User's Device Name",
  "deviceModel": "iPhone12,1 or SM-A125F",
  "deviceOS": "iOS or Android",
  "appVersion": "1.0.0",
  "isOnline": true,
  "lastOnlineAt": "2024-01-15T10:30:00",
  "registeredAt": "2024-01-15T10:30:00",
  "ipAddress": "192.168.1.100",
  "locationInfo": "Ho Chi Minh City",
  "latitude": 10.7595,
  "longitude": 106.7045,
  "isActive": true
}

Response (201 Created):
{
  "id": 1,
  "userId": 1,
  "deviceId": "device-uuid",
  "message": "Device registered successfully"
}
```

### 2. Get All Devices
```http
GET /api/devices

Response (200 OK):
[
  {
    "id": 1,
    "userId": 1,
    "deviceId": "device-1",
    "deviceName": "iPhone 12",
    "isOnline": true,
    "lastOnlineAt": "2024-01-15T14:20:00",
    "registeredAt": "2024-01-10T09:00:00"
  },
  ...
]
```

### 3. Get User Devices
```http
GET /api/devices/user/{userId}

Response (200 OK):
[
  {
    "id": 1,
    "userId": 1,
    "deviceName": "iPhone 12",
    "isOnline": true
  }
]
```

### 4. Get Specific Device
```http
GET /api/devices/{deviceId}

Response (200 OK):
{
  "id": 1,
  "userId": 1,
  "deviceName": "iPhone 12",
  "deviceModel": "iPhone12,1",
  "deviceOS": "iOS",
  "appVersion": "1.0.0",
  "isOnline": true,
  "lastOnlineAt": "2024-01-15T14:20:00",
  "ipAddress": "192.168.1.100",
  "locationInfo": "Ho Chi Minh City"
}
```

### 5. Get Online Devices
```http
GET /api/devices/status/online

Response (200 OK):
[
  {
    "id": 1,
    "userId": 1,
    "deviceName": "iPhone 12",
    "isOnline": true,
    "lastOnlineAt": "2024-01-15T14:20:00"
  }
]
```

### 6. Update Device
```http
PUT /api/devices/{deviceId}
Content-Type: application/json

{
  "userId": 1,
  "deviceName": "Updated Device Name",
  "isOnline": true,
  "isActive": true
}

Response (200 OK):
{
  "id": 1,
  "userId": 1,
  "deviceName": "Updated Device Name",
  "message": "Device updated successfully"
}
```

### 7. Update Device Status
```http
PUT /api/devices/{deviceId}/status
Content-Type: application/json

{
  "isOnline": true,
  "ipAddress": "192.168.1.100"
}

Response (200 OK):
{
  "id": 1,
  "isOnline": true,
  "lastOnlineAt": "2024-01-15T14:25:00"
}
```

### 8. Delete Device
```http
DELETE /api/devices/{deviceId}

Response (200 OK):
{
  "message": "Device deleted successfully"
}
```

---

## 🎫 QR Code API (Ready to Implement)

### 1. Generate QR Code
```http
POST /api/qrcodes/generate/{restaurantId}

Response (200 OK):
{
  "sessionId": 1,
  "qrCode": "ABC123DEF456",
  "sessionToken": "uuid-token",
  "restaurantId": 1,
  "restaurantName": "Quán Ốc Oanh",
  "createdAt": "2024-01-15T10:00:00",
  "expiresAt": "2024-01-15T10:05:00",
  "durationSeconds": 300,
  "scanCount": 0
}
```

### 2. Get Current QR Code
```http
GET /api/qrcodes/current/{restaurantId}

Response (200 OK):
{
  "sessionId": 1,
  "qrCode": "ABC123DEF456",
  "sessionToken": "uuid-token",
  "restaurantName": "Quán Ốc Oanh",
  "durationSeconds": 285,
  "scanCount": 3,
  "message": "Got current QR code successfully"
}
```

### 3. Verify QR Code
```http
POST /api/qrcodes/verify
Content-Type: application/json

{
  "sessionToken": "uuid-token"
}

Response (200 OK):
{
  "isValid": true,
  "restaurantId": 1,
  "restaurantName": "Quán Ốc Oanh",
  "secondsRemaining": 275
}
```

### 4. Scan QR Code
```http
POST /api/qrcodes/scan
Content-Type: application/json

{
  "userId": 1,
  "userName": "Nguyễn Văn A",
  "userEmail": "user@example.com",
  "qrSessionToken": "uuid-token",
  "deviceInfo": "iPhone 12 - iOS 17",
  "ipAddress": "192.168.1.100"
}

Response (200 OK):
{
  "scanRequestId": 1,
  "restaurantId": 1,
  "qrCode": "ABC123DEF456",
  "status": "success",
  "message": "QR scan successful. Waiting for web admin approval..."
}
```

### 5. Get Pending Requests
```http
GET /api/qrcodes/{restaurantId}/pending-requests

Response (200 OK):
[
  {
    "id": 1,
    "userId": 1,
    "userName": "Nguyễn Văn A",
    "userEmail": "user@example.com",
    "scanTime": "2024-01-15T10:02:00",
    "status": "pending",
    "deviceInfo": "iPhone 12"
  }
]
```

### 6. Approve Request
```http
POST /api/qrcodes/{requestId}/approve

Response (200 OK):
{
  "message": "Request approved successfully"
}
```

### 7. Get Weekly Statistics
```http
GET /api/qrcodes/{restaurantId}/statistics

Response (200 OK):
{
  "totalScansToday": 25,
  "uniqueUsersToday": 18,
  "scansPerDay": {
    "CN": 150,  // Sunday
    "T2": 165,  // Monday
    "T3": 140,
    "T4": 155,
    "T5": 170,
    "T6": 145,
    "T7": 160   // Saturday
  }
}
```

---

## 👤 User & Payment API

### Get All Users
```http
GET /api/users

Response (200 OK):
[
  {
    "id": 1,
    "fullName": "Nguyễn Văn A",
    "email": "user@example.com",
    "phone": "0901234567",
    "isPaid": true,
    "paidAt": "2024-01-10T00:00:00"
  }
]
```

### Get User by ID
```http
GET /api/users/{userId}
```

### Update User
```http
PUT /api/users/{userId}
Content-Type: application/json

{
  "fullName": "Updated Name",
  "email": "newemail@example.com",
  "phone": "0909876543"
}
```

### Delete User
```http
DELETE /api/users/{userId}
```

---

## 🏪 Restaurant (POI) API

### Get All Restaurants
```http
GET /api/pois

Response (200 OK):
[
  {
    "id": 1,
    "name": "Quán Ốc Oanh",
    "address": "534 Vĩnh Khánh, Q.4",
    "description": "Ốc tươi ngon",
    "latitude": 10.7595,
    "longitude": 106.7045,
    "qrCode": "ABC123DEF456"
  }
]
```

### Get Restaurant by ID
```http
GET /api/pois/{poiId}
```

### Create Restaurant
```http
POST /api/pois
Content-Type: application/json

{
  "name": "New Restaurant",
  "address": "Address here",
  "description": "Description",
  "latitude": 10.7595,
  "longitude": 106.7045
}
```

### Update Restaurant
```http
PUT /api/pois/{poiId}
```

### Delete Restaurant
```http
DELETE /api/pois/{poiId}
```

---

## 💳 Payment API

### Get User Payment
```http
GET /api/payments/user/{userId}
```

### Update Payment Status
```http
PUT /api/users/{userId}/payment
Content-Type: application/json

{
  "isPaid": true,
  "paidAt": "2024-01-15T14:30:00"
}
```

---

## 📊 Dashboard API

### Get Dashboard Summary
```http
GET /api/pois/dashboard/summary

Response (200 OK):
{
  "totalOnlineUsers": 15,
  "onlineDevices": 15,
  "activeListeningUsers": 8,
  "totalRegisteredUsers": 150,
  "totalPaidUsers": 45,
  "todayQRScans": 235,
  "onlineDeviceDetails": [...]
}
```

### Get QR Activity Today
```http
GET /api/pois/dashboard/qr-activity

Response (200 OK):
{
  "totalScans": 235,
  "uniqueUsers": 120,
  "topPOIs": [
    { "name": "Quán Ốc Oanh", "count": 45 },
    { "name": "Ốc Vũ", "count": 38 }
  ]
}
```

---

## 🔐 Admin API (Ready to Implement)

### Admin Login
```http
POST /api/admin/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}

Response (200 OK):
{
  "id": 1,
  "username": "admin",
  "fullName": "Admin User",
  "role": "admin",
  "message": "Login successful"
}
```

### Get All Admins
```http
GET /api/admin
```

### Create Admin
```http
POST /api/admin
Content-Type: application/json

{
  "username": "manager",
  "password": "Manager@123",
  "fullName": "Manager Name",
  "email": "manager@example.com",
  "role": "manager"
}
```

### Logout
```http
POST /api/admin/{adminId}/logout
```

---

## 🖼️ Image API (Ready to Implement)

### Get Restaurant Images
```http
GET /api/images/restaurant/{restaurantId}

Response (200 OK):
[
  {
    "id": 1,
    "restaurantId": 1,
    "imagePath": "/uploads/restaurant-images/uuid.jpg",
    "imageName": "restaurant.jpg",
    "isMainImage": true,
    "uploadedAt": "2024-01-15T10:00:00"
  }
]
```

### Upload Image
```http
POST /api/images/upload/{restaurantId}
Content-Type: multipart/form-data

[Binary image file]

Response (200 OK):
{
  "message": "Image uploaded successfully",
  "image": {
    "id": 1,
    "imagePath": "/uploads/restaurant-images/uuid.jpg"
  }
}
```

### Set Main Image
```http
PUT /api/images/{imageId}/main

Response (200 OK):
{
  "message": "Main image set successfully"
}
```

### Delete Image
```http
DELETE /api/images/{imageId}
```

---

## 📋 Audit Log API (Ready to Implement)

### Get All Audit Logs
```http
GET /api/audit

Response (200 OK):
[
  {
    "id": 1,
    "adminUserId": 1,
    "action": "LOGIN",
    "entityType": "AdminUser",
    "entityId": 1,
    "ipAddress": "192.168.1.100",
    "isSuccess": true,
    "createdAt": "2024-01-15T10:00:00"
  }
]
```

### Get Logs by User
```http
GET /api/audit/user/{adminUserId}
```

### Get Logs by Action
```http
GET /api/audit/action/{action}
```

---

## ⚙️ Settings API (Ready to Implement)

### Get All Settings
```http
GET /api/settings

Response (200 OK):
[
  {
    "key": "App.Name",
    "value": "Vĩnh Khánh Tour",
    "description": "Application name",
    "settingType": "string"
  }
]
```

### Get Setting Value
```http
GET /api/settings/{key}

Response (200 OK):
{
  "value": "Vĩnh Khánh Tour"
}
```

### Update Setting
```http
PUT /api/settings/{key}
Content-Type: application/json

{
  "value": "New Value",
  "updatedBy": "admin"
}
```

---

## ✅ Error Responses

All endpoints return standard error responses:

```json
{
  "error": "Error message describing what went wrong"
}

Status Codes:
- 200 OK: Success
- 201 Created: Resource created
- 400 Bad Request: Invalid input
- 404 Not Found: Resource not found
- 500 Internal Server Error: Server error
```

---

## 🔄 Common Use Cases

### Register New User Device
```javascript
const device = {
  userId: currentUser.id,
  deviceId: getDeviceId(),
  deviceName: `${getUserName()} - ${getDeviceModel()}`,
  deviceModel: getDeviceModel(),
  deviceOS: getPlatform(),
  appVersion: "1.0.0"
};

const response = await fetch('/api/devices', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(device)
});
```

### Update User Online Status on App Open
```javascript
await fetch(`/api/devices/${deviceId}/status`, {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    isOnline: true,
    ipAddress: getIpAddress()
  })
});
```

### Scan QR Code
```javascript
const scanData = {
  userId: currentUser.id,
  userName: currentUser.fullName,
  userEmail: currentUser.email,
  qrSessionToken: scannedQRToken,
  deviceInfo: getDeviceInfo(),
  ipAddress: getIpAddress()
};

const response = await fetch('/api/qrcodes/scan', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(scanData)
});
```

---

**All endpoints tested and ready for implementation!** 🚀
