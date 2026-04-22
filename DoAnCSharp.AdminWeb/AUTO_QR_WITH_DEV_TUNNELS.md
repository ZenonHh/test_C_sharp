# \ud83d\ude80 **H\u01af\u1edaNG D\u1eaaN SETUP DEV TUNNELS - T\u1ef0 \u0110\u1ed8NG T\u1ea0O QR V\u1edaI TUNNEL URL**

## \u2728 **T\u00cdNH N\u0102NG**

Sau khi setup xong, **m\u1ecdi khi t\u1ea1o POI m\u1edbi**, QR code s\u1ebd t\u1ef1 \u0111\u1ed9ng d\u00f9ng **Dev Tunnels URL** th\u00e1y v\u00ec local IP!

### **Tr\u01b0\u1edbc khi setup:**
```
QR Code c\u0169: http://192.168.1.100:5000/qr/POI_UA8AG0H2D
\u2705 Ch\u1ec9 qu\u00e9t \u0111\u01b0\u1ee3c khi c\u00f9ng m\u1ea1ng WiFi
```

### **Sau khi setup:**
```
QR Code m\u1edbi: https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
\u2705 Qu\u00e9t \u0111\u01b0\u1ee3c t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u tr\u00ean th\u1ebf gi\u1edbi!
```

---

## \ud83d\ude80 **SETUP T\u1ef0 \u0110\u1ed8NG (5 PH\u00daT)**

### **B\u01b0\u1edbc 1: Ch\u1ea1y Script Setup**

**M\u1edf PowerShell t\u1ea1i th\u01b0 m\u1ee5c g\u1ed1c d\u1ef1 \u00e1n:**
```powershell
cd C:\Users\LENOVO\source\repos\do_an_C_sharp
```

**Ch\u1ea1y script t\u1ef1 \u0111\u1ed9ng:**
```powershell
.\DoAnCSharp.AdminWeb\setup-dev-tunnel-auto.ps1
```

**Script s\u1ebd t\u1ef1 \u0111\u1ed9ng:**
1. \u2705 Ki\u1ec3m tra `devtunnel` CLI (\u0111\u1ec1 ngh\u1ecb c\u00e0i n\u1ebfu ch\u01b0a c\u00f3)
2. \u2705 \u0110\u0103ng nh\u1eadp Microsoft Account
3. \u2705 T\u1ea1o tunnel `vinhkhanh-tour` (n\u1ebfu ch\u01b0a c\u00f3)
4. \u2705 C\u1ea5u h\u00ecnh port 5000
5. \u2705 L\u01b0u tunnel URL v\u00e0o bi\u1ebfn m\u00f4i tr\u01b0\u1eddng
6. \u2705 T\u1ea1o file `.env` v\u1edbi c\u1ea5u h\u00ecnh

**K\u1ebft qu\u1ea3:**
```
\ud83c\udf10 TUNNEL URL: https://vinhkhanh-tour-abc123.devtunnels.ms
\u2705 \u0110\u00e3 l\u01b0u bi\u1ebfn m\u00f4i tr\u01b0\u1eddng: DEV_TUNNEL_URL
```

### **B\u01b0\u1edbc 2: Ch\u1ea1y Server**

**M\u1edf terminal m\u1edbi (kh\u00f4ng \u0111\u00f3ng terminal script):**
```powershell
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

**Ch\u1edd server ch\u1ea1y:**
```
Now listening on: http://0.0.0.0:5000
```

### **B\u01b0\u1edbc 3: K\u1ebft n\u1ed1i Tunnel**

**Quay l\u1ea1i terminal ch\u1ea1y script, nh\u1ea5n ENTER:**
```
Nh\u1ea5n ENTER \u0111\u1ec3 k\u1ebft n\u1ed1i tunnel...
```

**Tunnel s\u1ebd k\u1ebft n\u1ed1i:**
```
\ud83c\udf10 PUBLIC URL: https://vinhkhanh-tour-abc123.devtunnels.ms
\u2705 Tunnel \u0111ang ho\u1ea1t \u0111\u1ed9ng!
```

**\u26a0\ufe0f  \u0110\u1eebng \u0111\u00f3ng terminal n\u00e0y! Gi\u1eef m\u1edf \u0111\u1ec3 tunnel ho\u1ea1t \u0111\u1ed9ng.**

---

## \ud83c\udfaf **S\u1ed0 D\u1ee4NG - T\u1ea0O QR CODE M\u1edaI**

### **C\u00e1ch 1: T\u1ea1o POI m\u1edbi (T\u1ef0 \u0111\u1ed9ng d\u00f9ng Tunnel URL)**

1. **Truy c\u1eadp Admin Dashboard:**
   ```
   https://vinhkhanh-tour-abc123.devtunnels.ms
   ```

2. **V\u00e0o tab "\ud83c\udfea Qu\u00e1n \u0102n"**

3. **Click "Th\u00eam m\u1edbi"**

4. **\u0110i\u1ec1n th\u00f4ng tin:**
   - T\u00ean: \u1ed0c Oanh Vi\u1ec7t Nam
   - \u0110\u1ecba ch\u1ec9: 123 Nguy\u1ec5n Hu\u1ec7, Q1, TP.HCM
   - M\u00f4 t\u1ea3: Qu\u00e1n \u1ed1c n\u1ed5i ti\u1ebfng...

5. **\u0110\u1ec3 tr\u1ed1ng "\u200bM\u00e3 QR"** (s\u1ebd t\u1ef1 \u0111\u1ed9ng t\u1ea1o)

6. **Click "L\u01b0u"**

**K\u1ebft qu\u1ea3:**
```json
{
  "id": 6,
  "name": "\u1ed0c Oanh Vi\u1ec7t Nam",
  "qrCode": "https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_A1B2C3D4E5"
}
```

\u2705 **QR code t\u1ef1 \u0111\u1ed9ng d\u00f9ng Tunnel URL!**

### **C\u00e1ch 2: Xem QR code c\u1ee7a POI hi\u1ec7n c\u00f3**

1. **V\u00e0o tab "Qu\u00e1n \u0102n"**
2. **Click "Xem QR" tr\u00ean POI b\u1ea5t k\u1ef3**
3. **QR code hi\u1ec3n th\u1ecb v\u1edbi full URL**
4. **Click "\ud83d\udcbe T\u1ea3i QR Code"** \u0111\u1ec3 download

### **C\u00e1ch 3: Update POI c\u0169 sang Tunnel URL**

**N\u1ebfu b\u1ea1n c\u00f3 POI c\u0169 v\u1edbi QR code local:**

1. **Click "S\u1eeda" tr\u00ean POI c\u0169**
2. **X\u00f3a n\u1ed9i dung "\u200bM\u00e3 QR"** (ho\u1eb7c \u0111\u1ec3 nguy\u00ean)
3. **Click "L\u01b0u"**

**H\u1ec7 th\u1ed1ng s\u1ebd:**
- N\u1ebfu QR tr\u1ed1ng \u2192 T\u1ea1o m\u1edbi v\u1edbi Tunnel URL
- N\u1ebfu QR l\u00e0 code (kh\u00f4ng ph\u1ea3i URL) \u2192 Convert sang Tunnel URL
- N\u1ebfu QR \u0111\u00e3 l\u00e0 URL \u2192 Gi\u1eef nguy\u00ean

---

## \ud83d\udcf1 **TEST QR T\u1eea SMARTPHONE**

### **B\u01b0\u1edbc 1: T\u1ea1o QR code**

**Truy c\u1eadp Admin Dashboard v\u00e0 t\u1ea1o POI m\u1edbi:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms
\u2192 Tab "Qu\u00e1n \u0102n" \u2192 Th\u00eam m\u1edbi \u2192 L\u01b0u
```

**QR code \u0111\u01b0\u1ee3c t\u1ea1o:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_XYZ123
```

### **B\u01b0\u1edbc 2: Download QR**

**Click "Xem QR" \u2192 "T\u1ea3i QR Code" \u2192 L\u01b0u file PNG**

### **B\u01b0\u1edbc 3: Test qu\u00e9t**

**T\u1eaft WiFi tr\u00ean \u0111i\u1ec7n tho\u1ea1i (d\u00f9ng 4G/5G):**

1. **M\u1edf Camera** tr\u00ean \u0111i\u1ec7n tho\u1ea1i
2. **Qu\u00e9t QR code**
3. **Nh\u1ea5n v\u00e0o link hi\u1ec3n th\u1ecb**

**K\u1ebft qu\u1ea3:**
```
\u2705 Trang hi\u1ec3n th\u1ecb th\u00f4ng tin qu\u00e1n ngay l\u1eadp t\u1ee9c!
\u2705 Kh\u00f4ng c\u1ea7n c\u00f9ng m\u1ea1ng WiFi!
\u2705 Qu\u00e9t \u0111\u01b0\u1ee3c t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u!
```

---

## \u2699\ufe0f **C\u01a0 CH\u1ebe HO\u1ea0T \u0110\u1ed8NG**

### **1. T\u1ef1 \u0111\u1ed9ng detect Tunnel URL**

**Khi t\u1ea1o QR, h\u1ec7 th\u1ed1ng ki\u1ec3m tra theo th\u1ee9 t\u1ef1:**

1. **Priority 1: Bi\u1ebfn m\u00f4i tr\u01b0\u1eddng `DEV_TUNNEL_URL`**
   ```
   \u2705 N\u1ebfu c\u00f3 \u2192 D\u00f9ng Tunnel URL
   ```

2. **Priority 2: C\u1ea5u h\u00ecnh `ServerSettings:PublicUrl`**
   ```
   \u2705 N\u1ebfu c\u00f3 \u2192 D\u00f9ng Public URL t\u1eeb config
   ```

3. **Priority 3: Request Host**
   ```
   \u26a0\ufe0f  Fallback \u2192 D\u00f9ng URL hi\u1ec7n t\u1ea1i (local)
   ```

### **2. L\u01b0u bi\u1ebfn m\u00f4i tr\u01b0\u1eddng**

**Script t\u1ef1 \u0111\u1ed9ng l\u01b0u:**

**Session hi\u1ec7n t\u1ea1i:**
```powershell
$env:DEV_TUNNEL_URL = "https://vinhkhanh-tour-abc123.devtunnels.ms"
```

**V\u0129nh vi\u1ec5n (User Environment):**
```powershell
[System.Environment]::SetEnvironmentVariable("DEV_TUNNEL_URL", "...", "User")
```

**File .env (cho project):**
```
DEV_TUNNEL_URL=https://vinhkhanh-tour-abc123.devtunnels.ms
PUBLIC_URL=https://vinhkhanh-tour-abc123.devtunnels.ms
```

### **3. Code t\u1ef1 \u0111\u1ed9ng t\u1ea1o QR**

**DatabaseService.cs:**
```csharp
public async Task<string> GetCurrentBaseUrlAsync()
{
    // Priority 1: Dev Tunnel
    var tunnelUrl = Environment.GetEnvironmentVariable("DEV_TUNNEL_URL");
    if (!string.IsNullOrEmpty(tunnelUrl))
        return tunnelUrl.TrimEnd('/');
    
    // Priority 2: Public URL
    var publicUrl = Environment.GetEnvironmentVariable("PUBLIC_URL");
    if (!string.IsNullOrEmpty(publicUrl))
        return publicUrl.TrimEnd('/');
    
    // Priority 3: Default
    return "http://172.20.10.2:5000";
}
```

**POIsController.cs:**
```csharp
private async Task<string> GenerateQRCodeAsync()
{
    string baseCode = "POI_" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
    
    // T\u1ef1 \u0111\u1ed9ng detect tunnel URL
    var tunnelUrl = Environment.GetEnvironmentVariable("DEV_TUNNEL_URL");
    if (!string.IsNullOrEmpty(tunnelUrl))
    {
        return $"{tunnelUrl.TrimEnd('/')}/qr/{baseCode}";
    }
    
    // Fallback
    return $"{Request.Scheme}://{Request.Host}/qr/{baseCode}";
}
```

---

## \ud83d\udc1b **TROUBLESHOOTING**

### **L\u1ed7i 1: "devtunnel not found"**

**Nguy\u00ean nh\u00e2n:** Ch\u01b0a c\u00e0i devtunnel CLI

**Gi\u1ea3i ph\u00e1p:**
```powershell
winget install Microsoft.devtunnel
```

**Ho\u1eb7c t\u1ea3i th\u1ee7 c\u00f4ng:** https://aka.ms/devtunnels/download

### **L\u1ed7i 2: "Not logged in"**

**Nguy\u00ean nh\u00e2n:** Ch\u01b0a \u0111\u0103ng nh\u1eadp Microsoft Account

**Gi\u1ea3i ph\u00e1p:**
```powershell
devtunnel user login
```

### **L\u1ed7i 3: QR v\u1eabn d\u00f9ng local URL**

**Nguy\u00ean nh\u00e2n:** Bi\u1ebfn m\u00f4i tr\u01b0\u1eddng ch\u01b0a \u0111\u01b0\u1ee3c set

**Gi\u1ea3i ph\u00e1p:**

**Ki\u1ec3m tra bi\u1ebfn m\u00f4i tr\u01b0\u1eddng:**
```powershell
$env:DEV_TUNNEL_URL
```

**N\u1ebfu tr\u1ed1ng, set l\u1ea1i:**
```powershell
$env:DEV_TUNNEL_URL = "https://vinhkhanh-tour-abc123.devtunnels.ms"
```

**Restart server sau khi set:**
```powershell
# Ctrl+C \u0111\u1ec3 d\u1eebng server
dotnet run
```

### **L\u1ed7i 4: "Port already in use"**

**Nguy\u00ean nh\u00e2n:** Port 5000 \u0111ang \u0111\u01b0\u1ee3c s\u1eed d\u1ee5ng

**Gi\u1ea3i ph\u00e1p:**
```powershell
# T\u00ecm process \u0111ang d\u00f9ng port
netstat -ano | findstr :5000

# Kill process
taskkill /PID <PID> /F
```

### **L\u1ed7i 5: Tunnel disconnect**

**Nguy\u00ean nh\u00e2n:** \u0110\u00f3ng terminal ch\u1ea1y tunnel

**Gi\u1ea3i ph\u00e1p:**
```powershell
# Ch\u1ea1y l\u1ea1i tunnel
cd DoAnCSharp.AdminWeb
.\setup-dev-tunnel-auto.ps1
```

---

## \ud83d\udcca **SO S\u00c1NH TR\u01af\u1edaC/SAU**

### **\u274c TR\u01af\u1edaC KHI SETUP:**

**QR Code:**
```
http://192.168.1.100:5000/qr/POI_UA8AG0H2D
```

**Gi\u1edbi h\u1ea1n:**
- \u274c Ch\u1ec9 qu\u00e9t \u0111\u01b0\u1ee3c khi c\u00f9ng m\u1ea1ng WiFi
- \u274c Kh\u00f4ng qu\u00e9t \u0111\u01b0\u1ee3c t\u1eeb 4G/5G
- \u274c Kh\u00f4ng chia s\u1ebb \u0111\u01b0\u1ee3c v\u1edbi ng\u01b0\u1eddi kh\u00e1c

### **\u2705 SAU KHI SETUP:**

**QR Code:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms/qr/POI_UA8AG0H2D
```

**\u01afa th\u1ebf:**
- \u2705 Qu\u00e9t \u0111\u01b0\u1ee3c t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u (4G/5G/WiFi kh\u00e1c)
- \u2705 Chia s\u1ebb v\u1edbi kh\u00e1ch h\u00e0ng, b\u1ea1n b\u00e8
- \u2705 HTTPS t\u1ef1 \u0111\u1ed9ng (b\u1ea3o m\u1eadt)
- \u2705 Persistent URL (kh\u00f4ng \u0111\u1ed5i)

---

## \ud83c\udfaf **WORKFLOW HO\u00c0N CH\u1ec8NH**

### **Scenario: T\u1ea1o QR cho qu\u00e1n m\u1edbi**

**1. Setup Dev Tunnels (Ch\u1ec9 l\u00e0m 1 l\u1ea7n):**
```powershell
.\DoAnCSharp.AdminWeb\setup-dev-tunnel-auto.ps1
```

**2. Ch\u1ea1y server (M\u1ed7i l\u1ea7n dev):**
```powershell
cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb
dotnet run
```

**3. K\u1ebft n\u1ed1i tunnel (M\u1ed7i l\u1ea7n dev):**
```powershell
# Quay l\u1ea1i terminal setup, nh\u1ea5n ENTER
```

**4. T\u1ea1o POI:**
```
https://vinhkhanh-tour-abc123.devtunnels.ms
\u2192 Qu\u00e1n \u0102n \u2192 Th\u00eam m\u1edbi
\u2192 \u0110i\u1ec1n th\u00f4ng tin \u2192 L\u01b0u
```

**5. Download QR:**
```
\u2192 Xem QR \u2192 T\u1ea3i QR Code
```

**6. Test:**
```
\u2192 In QR ho\u1eb7c hi\u1ec3n th\u1ecb tr\u00ean m\u00e0n h\u00ecnh
\u2192 Qu\u00e9t b\u1eb1ng \u0111i\u1ec7n tho\u1ea1i (b\u1ea5t k\u1ef3 m\u1ea1ng n\u00e0o)
\u2192 \u2705 Th\u00e0nh c\u00f4ng!
```

---

## \ud83c\udf89 **K\u1ebeT LU\u1eacN**

**\u2705 \u0110\u00e3 c\u00e0i \u0111\u1eb7t:**
- Dev Tunnels t\u1ef1 \u0111\u1ed9ng
- Bi\u1ebfn m\u00f4i tr\u01b0\u1eddng `DEV_TUNNEL_URL`
- T\u1ef1 \u0111\u1ed9ng t\u1ea1o QR v\u1edbi Tunnel URL

**\u2705 B\u1ea1n c\u00f3 th\u1ec3:**
- T\u1ea1o QR code qu\u00e9t \u0111\u01b0\u1ee3c t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u
- Chia s\u1ebb QR v\u1edbi kh\u00e1ch h\u00e0ng, b\u1ea1n b\u00e8
- Test tr\u00ean thi\u1ebft b\u1ecb th\u1ef1c (kh\u00f4ng c\u1ea7n simulator)

**\ud83d\ude80 Gi\u1edd \u0111\u00e2y b\u1ea1n c\u00f3 th\u1ec3 qu\u00e9t QR t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u tr\u00ean th\u1ebf gi\u1edbi!**

---

**\ud83d\udcda T\u00e0i li\u1ec7u tham kh\u1ea3o:**
- **Setup chi ti\u1ebft:** `DEV_TUNNELS_GUIDE.md`
- **Quick start:** `DEV_TUNNELS_QUICK.md`
- **Troubleshooting:** `DEV_TUNNELS_GUIDE.md` (ph\u1ea7n "\ud83d\udc1b X\u1eed l\u00fd l\u1ed7i")
