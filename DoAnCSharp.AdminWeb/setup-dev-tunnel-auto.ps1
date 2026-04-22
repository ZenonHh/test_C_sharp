# ============================================
# Dev Tunnels Auto-Setup for VinhKhanh Tour
# T\u1ef1 \u0111\u1ed9ng t\u1ea1o tunnel v\u00e0 set URL v\u00e0o bi\u1ebfn m\u00f4i tr\u01b0\u1eddng
# ============================================

Write-Host ""
Write-Host "\ud83c\udf10 VINHKHANH TOUR - DEV TUNNELS AUTO SETUP" -ForegroundColor Cyan
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host ""

# Check if devtunnel CLI is installed
$devtunnelInstalled = Get-Command devtunnel -ErrorAction SilentlyContinue

if (-not $devtunnelInstalled) {
    Write-Host "\u274c devtunnel CLI ch\u01b0a \u0111\u01b0\u1ee3c c\u00e0i \u0111\u1eb7t!" -ForegroundColor Red
    Write-Host ""
    Write-Host "\ud83d\udce6 H\u01b0\u1edbng d\u1eabn c\u00e0i \u0111\u1eb7t:" -ForegroundColor Yellow
    Write-Host "  1. M\u1edf Visual Studio 2022" -ForegroundColor White
    Write-Host "  2. Tools \u2192 Options \u2192 Environment \u2192 Preview Features" -ForegroundColor White
    Write-Host "  3. B\u1eadt 'Dev Tunnels'" -ForegroundColor White
    Write-Host ""
    Write-Host "  Ho\u1eb7c c\u00e0i qua winget:" -ForegroundColor White
    Write-Host "  winget install Microsoft.devtunnel" -ForegroundColor White
    Write-Host ""
    
    # Ask if user wants to install
    $install = Read-Host "B\u1ea1n c\u00f3 mu\u1ed1n c\u00e0i \u0111\u1eb7t ngay kh\u00f4ng? (Y/N)"
    if ($install -eq 'Y' -or $install -eq 'y') {
        Write-Host "\ud83d\udce6 \u0110ang c\u00e0i \u0111\u1eb7t devtunnel CLI..." -ForegroundColor Yellow
        winget install Microsoft.devtunnel
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "\u274c C\u00e0i \u0111\u1eb7t th\u1ea5t b\u1ea1i!" -ForegroundColor Red
            Write-Host "H\u00e3y t\u1ea3i th\u1ee7 c\u00f4ng t\u1ea1i: https://aka.ms/devtunnels/download" -ForegroundColor Yellow
            exit 1
        }
        
        Write-Host "\u2705 C\u00e0i \u0111\u1eb7t th\u00e0nh c\u00f4ng!" -ForegroundColor Green
        Write-Host ""
    } else {
        exit 1
    }
}

Write-Host "\u2705 devtunnel CLI \u0111\u00e3 c\u00e0i \u0111\u1eb7t" -ForegroundColor Green
Write-Host ""

# Check login status
Write-Host "\ud83d\udd10 Ki\u1ec3m tra \u0111\u0103ng nh\u1eadp..." -ForegroundColor Yellow
$loginCheck = devtunnel user show 2>&1

if ($loginCheck -like "*not logged in*" -or $LASTEXITCODE -ne 0) {
    Write-Host "\u26a0\ufe0f  Ch\u01b0a \u0111\u0103ng nh\u1eadp. \u0110ang m\u1edf tr\u00ecnh duy\u1ec7t \u0111\u1ec3 \u0111\u0103ng nh\u1eadp..." -ForegroundColor Yellow
    devtunnel user login
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "\u274c \u0110\u0103ng nh\u1eadp th\u1ea5t b\u1ea1i!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "\u2705 \u0110\u00e3 \u0111\u0103ng nh\u1eadp" -ForegroundColor Green
Write-Host ""

# Check existing tunnels
Write-Host "\ud83d\udd0d Ki\u1ec3m tra tunnels hi\u1ec7n c\u00f3..." -ForegroundColor Yellow
$existingTunnels = devtunnel list --output json 2>&1

try {
    $tunnels = $existingTunnels | ConvertFrom-Json
} catch {
    $tunnels = @()
}

$tunnelName = "vinhkhanh-tour"
$tunnel = $tunnels | Where-Object { $_.name -eq $tunnelName }

if (-not $tunnel) {
    Write-Host "\ud83d\udcdd T\u1ea1o tunnel m\u1edbi: $tunnelName" -ForegroundColor Yellow
    $createResult = devtunnel create $tunnelName --allow-anonymous 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "\u274c T\u1ea1o tunnel th\u1ea5t b\u1ea1i!" -ForegroundColor Red
        Write-Host $createResult -ForegroundColor Red
        exit 1
    }
    
    # Get tunnel ID from result
    Start-Sleep -Seconds 2
    $tunnels = devtunnel list --output json | ConvertFrom-Json
    $tunnel = $tunnels | Where-Object { $_.name -eq $tunnelName }
}

Write-Host "\u2705 Tunnel \u0111\u00e3 s\u1eb5n s\u00e0ng: $($tunnel.name)" -ForegroundColor Green
Write-Host "   Tunnel ID: $($tunnel.tunnelId)" -ForegroundColor Cyan
Write-Host ""

# Configure port
$serverPort = 5000
Write-Host "\ud83d\udd0c C\u1ea5u h\u00ecnh port $serverPort..." -ForegroundColor Yellow

# Check if port already exists
$portCheck = devtunnel port show $tunnel.tunnelId -p $serverPort 2>&1

if ($LASTEXITCODE -ne 0) {
    devtunnel port create $tunnel.tunnelId -p $serverPort 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "\u274c C\u1ea5u h\u00ecnh port th\u1ea5t b\u1ea1i!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "\u2705 Port $serverPort \u0111\u00e3 \u0111\u01b0\u1ee3c c\u1ea5u h\u00ecnh" -ForegroundColor Green
Write-Host ""

# Get tunnel URL
$tunnelUrl = "https://$($tunnel.tunnelId).devtunnels.ms"
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host "\ud83c\udf10 TUNNEL URL: $tunnelUrl" -ForegroundColor Green -BackgroundColor DarkGreen
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host ""

# Save to environment variables (both session and permanent)
Write-Host "\ud83d\udcbe L\u01b0u tunnel URL v\u00e0o bi\u1ebfn m\u00f4i tr\u01b0\u1eddng..." -ForegroundColor Yellow

# Current session
$env:DEV_TUNNEL_URL = $tunnelUrl

# User environment (permanent)
[System.Environment]::SetEnvironmentVariable("DEV_TUNNEL_URL", $tunnelUrl, "User")

Write-Host "\u2705 \u0110\u00e3 l\u01b0u bi\u1ebfn m\u00f4i tr\u01b0\u1eddng:" -ForegroundColor Green
Write-Host "   DEV_TUNNEL_URL = $tunnelUrl" -ForegroundColor Cyan
Write-Host ""

# Create .env file for project
$envFilePath = ".\DoAnCSharp.AdminWeb\.env"
@"
# Dev Tunnels Configuration
# Auto-generated by setup-dev-tunnel-auto.ps1
DEV_TUNNEL_URL=$tunnelUrl
PUBLIC_URL=$tunnelUrl
SERVER_PUBLIC_URL=$tunnelUrl

# Last updated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')
"@ | Out-File -FilePath $envFilePath -Encoding UTF8

Write-Host "\u2705 \u0110\u00e3 t\u1ea1o file .env t\u1ea1i: $envFilePath" -ForegroundColor Green
Write-Host ""

# Show instructions
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host "\ud83d\ude80 H\u01af\u1edaNG D\u1eaaN S\u1ed0 D\u1ee4NG" -ForegroundColor Yellow
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host ""
Write-Host "1\ufe0f\u20e3  M\u1ede TERMINAL M\u1edaI v\u00e0 ch\u1ea1y server:" -ForegroundColor White
Write-Host "   cd DoAnCSharp.AdminWeb\DoAnCSharp.AdminWeb" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "2\ufe0f\u20e3  K\u1ebeT N\u1ed0I TUNNEL (terminal n\u00e0y):" -ForegroundColor White
Write-Host "   Nh\u1ea5n ENTER \u0111\u1ec3 k\u1ebft n\u1ed1i tunnel..." -ForegroundColor Cyan
Write-Host ""
Write-Host "3\ufe0f\u20e3  TRUY C\u1eacP:" -ForegroundColor White
Write-Host "   Admin Dashboard: $tunnelUrl" -ForegroundColor Cyan
Write-Host "   Test QR: $tunnelUrl/qr/POI_UA8AG0H2D" -ForegroundColor Cyan
Write-Host ""
Write-Host "4\ufe0f\u20e3  T\u1ea0O QR CODE:" -ForegroundColor White
Write-Host "   - V\u00e0o Admin Dashboard \u2192 Tab 'Qu\u00e1n \u0102n'" -ForegroundColor Cyan
Write-Host "   - Click 'Th\u00eam m\u1edbi' ho\u1eb7c 'S\u1eeda' POI b\u1ea5t k\u1ef3" -ForegroundColor Cyan
Write-Host "   - QR code s\u1ebd t\u1ef1 \u0111\u1ed9ng d\u00f9ng tunnel URL!" -ForegroundColor Cyan
Write-Host "   - Download QR v\u00e0 qu\u00e9t t\u1eeb b\u1ea5t k\u1ef3 \u0111\u00e2u!" -ForegroundColor Cyan
Write-Host ""
Write-Host "=" * 70 -ForegroundColor Cyan
Write-Host ""
Write-Host "\u26a0\ufe0f  GHI CH\u00da QUAN TR\u1eccNG:" -ForegroundColor Red
Write-Host "   - Server ph\u1ea3i \u0111ang ch\u1ea1y tr\u00ean port $serverPort" -ForegroundColor Yellow
Write-Host "   - Gi\u1eef c\u1eeda s\u1ed5 n\u00e0y m\u1edf (kh\u00f4ng \u0111\u00f3ng)" -ForegroundColor Yellow
Write-Host "   - Nh\u1ea5n Ctrl+C \u0111\u1ec3 d\u1eebng tunnel" -ForegroundColor Yellow
Write-Host ""

# Ask user to continue
$continue = Read-Host "Nh\u1ea5n ENTER \u0111\u1ec3 k\u1ebft n\u1ed1i tunnel (ho\u1eb7c Ctrl+C \u0111\u1ec3 tho\u00e1t)"

if ([string]::IsNullOrEmpty($continue)) {
    Write-Host ""
    Write-Host "\ud83d\ude80 \u0110ang k\u1ebft n\u1ed1i tunnel..." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "=" * 70 -ForegroundColor Cyan
    Write-Host "\ud83c\udf10 PUBLIC URL: $tunnelUrl" -ForegroundColor Green -BackgroundColor DarkGreen
    Write-Host "=" * 70 -ForegroundColor Cyan
    Write-Host ""
    Write-Host "\u2705 Tunnel \u0111ang ho\u1ea1t \u0111\u1ed9ng!" -ForegroundColor Green
    Write-Host ""
    Write-Host "\ud83d\udce2 QR codes s\u1ebd t\u1ef1 \u0111\u1ed9ng d\u00f9ng URL n\u00e0y khi t\u1ea1o m\u1edbi!" -ForegroundColor Cyan
    Write-Host ""
    
    # Host the tunnel
    devtunnel host $tunnel.tunnelId
}
