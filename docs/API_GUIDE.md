# 📚 API Guide

## Web Admin API Endpoints

**Base URL**: `http://localhost:5000/api`

---

## Payments API

### Get User Payment Info
```http
GET /api/payments/user/{userId}
```

**Response**:
```json
{
  "id": 1,
  "userId": 1,
  "isPaid": true,
  "paidDate": "2024-01-15T10:30:00",
  "paymentMethod": "Credit Card",
  "amount": 49000
}
```

### Add Payment
```http
POST /api/payments/add
Content-Type: application/json

{
  "userId": 1,
  "amount": 49000,
  "paymentMethod": "Credit Card"
}
```

### Update Payment
```http
PUT /api/payments/update/{id}
Content-Type: application/json

{
  "amount": 49000,
  "paymentMethod": "Bank Transfer"
}
```

---

## QR Scans API

### Check QR Scan Limit
```http
GET /api/qrscans/check/{userId}/{isPaid}
```

**Response**:
```json
{
  "canScan": true,
  "scanCount": 3,
  "maxScans": 5,
  "remainingScansFree": 2,
  "isPaidUser": false
}
```

### Increment Scan Count
```http
POST /api/qrscans/increment/{userId}
```

### Get Scan History
```http
GET /api/qrscans/history/{userId}
```

---

## Users API

### Get User
```http
GET /api/users/{userId}
```

**Response**:
```json
{
  "id": 1,
  "fullName": "Nguyen Van A",
  "email": "user@example.com",
  "phone": "0901234567",
  "avatar": "avatar.jpg"
}
```

### Create User
```http
POST /api/users/create
Content-Type: application/json

{
  "fullName": "Nguyen Van A",
  "email": "user@example.com",
  "password": "secure123",
  "phone": "0901234567"
}
```

### Update User
```http
PUT /api/users/update/{id}
Content-Type: application/json

{
  "fullName": "Nguyen Van A",
  "phone": "0901234567",
  "avatar": "new_avatar.jpg"
}
```

### Delete User
```http
DELETE /api/users/delete/{id}
```

---

## POIs API (Points of Interest)

### Get All POIs
```http
GET /api/pois
```

### Get POI by ID
```http
GET /api/pois/{id}
```

### Create POI
```http
POST /api/pois/create
Content-Type: application/json

{
  "name": "Vinh Khanh Restaurant",
  "latitude": 10.7769,
  "longitude": 106.7009,
  "description": "Famous local cuisine",
  "audioUrl": "audio.mp3"
}
```

### Update POI
```http
PUT /api/pois/update/{id}
```

### Delete POI
```http
DELETE /api/pois/delete/{id}
```

---

## User Status API

### Get User Status
```http
GET /api/userstatus/user/{userId}
```

**Response**:
```json
{
  "userId": 1,
  "isOnline": true,
  "lastActivityTime": "2024-01-15T10:30:00",
  "currentLocation": "Restaurant A"
}
```

### Set User Online
```http
POST /api/userstatus/set-online/{userId}
Content-Type: application/json

{
  "status": "online",
  "location": "Restaurant A"
}
```

---

## History API

### Get User History
```http
GET /api/history/user/{userId}
```

### Get History Details
```http
GET /api/history/{historyId}
```

### Create History Entry
```http
POST /api/history/create
Content-Type: application/json

{
  "userId": 1,
  "poiId": 5,
  "playTime": 120,
  "playDate": "2024-01-15T10:30:00"
}
```

---

## Error Responses

### 400 Bad Request
```json
{
  "error": "Invalid request data"
}
```

### 404 Not Found
```json
{
  "error": "User not found"
}
```

### 500 Server Error
```json
{
  "error": "Internal server error"
}
```

---

## Authentication

Current version uses **no authentication** (for testing).

For production, add:
- JWT token-based auth
- API key validation
- Rate limiting

---

## Database Models

### UserPayment
```csharp
public class UserPayment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public DateTime PaidDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
}
```

### QRScanLimit
```csharp
public class QRScanLimit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ScanCount { get; set; }
    public int MaxScans { get; set; }
    public DateTime LastResetDate { get; set; }
    public bool IsPaidUser { get; set; }
}
```

### UserStatus
```csharp
public class UserStatus
{
    public int UserId { get; set; }
    public bool IsOnline { get; set; }
    public DateTime LastActivityTime { get; set; }
    public string CurrentLocation { get; set; }
}
```

---

## Testing with Postman

1. Import endpoints into Postman
2. Set base URL: `http://localhost:5000`
3. Test each endpoint
4. Verify responses

---

## Common Issues

**CORS Issues**:
- API allows all origins in development
- Configure in Production!

**Database Not Initialized**:
```powershell
dotnet run --project DoAnCSharp.AdminWeb
# Let it initialize on startup
```

---

**More help**: See TROUBLESHOOTING.md
