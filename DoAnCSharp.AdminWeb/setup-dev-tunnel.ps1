# ============================================
# Dev Tunnels Setup Script
# Tự động tạo tunnel và chạy server
# ============================================

Write-Host "🚀 VinhKhanh Tour - Dev Tunnels Setup" -ForegroundColor Cyan
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host ""

# Check if devtunnel CLI is installed
$devtunnelInstalled = Get-Command devtunnel -ErrorAction SilentlyContinue

if (-not $devtunnelInstalled) {
    Write-Host "❌ devtunnel CLI chưa được cài đặt!" -ForegroundColor Red
    Write-Host ""
    Write-Host "📦 Hướng dẫn cài đặt:" -ForegroundColor Yellow
    Write-Host "  1. Mở Visual Studio 2022" -ForegroundColor White
    Write-Host "  2. Tools → Options → Environment → Preview Features" -ForegroundColor White
    Write-Host "  3. Bật 'Dev Tunnels'" -ForegroundColor White
    Write-Host ""
    Write-Host "  Hoặc cài qua winget:" -ForegroundColor White
    Write-Host "  winget install Microsoft.devtunnel" -ForegroundColor White
    Write-Host ""
    exit 1
}

Write-Host "✅ devtunnel CLI đã cài đặt" -ForegroundColor Green
Write-Host ""

# Check login status
Write-Host "🔐 Kiểm tra đăng nhập..." -ForegroundColor Yellow
$loginCheck = devtunnel user show 2>&1

if ($loginCheck -like "*not logged in*" -or $LASTEXITCODE -ne 0) {
    Write-Host "⚠️  Chưa đăng nhập. Đang mở trình duyệt để đăng nhập..." -ForegroundColor Yellow
    devtunnel user login
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Đăng nhập thất bại!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "✅ Đã đăng nhập" -ForegroundColor Green
Write-Host ""

# Check existing tunnels
Write-Host "🔍 Kiểm tra tunnels hiện có..." -ForegroundColor Yellow
$existingTunnels = devtunnel list --output json 2>&1 | ConvertFrom-Json

$tunnelName = "vinhkhanh-tour"
$tunnel = $existingTunnels | Where-Object { $_.name -eq $tunnelName }

if (-not $tunnel) {
    Write-Host "📝 Tạo tunnel mới: $tunnelName" -ForegroundColor Yellow
    $createResult = devtunnel create $tunnelName --allow-anonymous 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Tạo tunnel thất bại!" -ForegroundColor Red
        Write-Host $createResult -ForegroundColor Red
        exit 1
    }
    
    # Get tunnel ID from result
    $tunnels = devtunnel list --output json | ConvertFrom-Json
    $tunnel = $tunnels | Where-Object { $_.name -eq $tunnelName }
}

Write-Host "✅ Tunnel đã sẵn sàng: $($tunnel.name)" -ForegroundColor Green
Write-Host "   Tunnel ID: $($tunnel.tunnelId)" -ForegroundColor Cyan
Write-Host ""

# Configure port
$serverPort = 5000
Write-Host "🔌 Cấu hình port $serverPort..." -ForegroundColor Yellow

# Check if port already exists
$existingPort = devtunnel port show $tunnel.tunnelId -p $serverPort --output json 2>&1 | ConvertFrom-Json

if (-not $existingPort -or $LASTEXITCODE -ne 0) {
    devtunnel port create $tunnel.tunnelId -p $serverPort 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Cấu hình port thất bại!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "✅ Port $serverPort đã được cấu hình" -ForegroundColor Green
Write-Host ""

# Get tunnel URL
$tunnelUrl = "https://$($tunnel.tunnelId).devtunnels.ms"
Write-Host "🌐 Tunnel URL: $tunnelUrl" -ForegroundColor Cyan -BackgroundColor DarkBlue
Write-Host ""

# Save to environment variable
$env:DEV_TUNNEL_URL = $tunnelUrl
[System.Environment]::SetEnvironmentVariable("DEV_TUNNEL_URL", $tunnelUrl, "User")
Write-Host "✅ Đã lưu tunnel URL vào biến môi trường" -ForegroundColor Green
Write-Host ""

# Show instructions
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host "📋 HƯỚNG DẪN SỬ DỤNG" -ForegroundColor Yellow
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host ""
Write-Host "1️⃣  MỞ TERMINAL MỚI và chạy server:" -ForegroundColor White
Write-Host "   cd DoAnCSharp.AdminWeb" -ForegroundColor Cyan
Write-Host "   dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "2️⃣  KẾT NỐI TUNNEL (terminal này):" -ForegroundColor White
Write-Host "   Nhấn ENTER để kết nối tunnel..." -ForegroundColor Cyan
Write-Host ""
Write-Host "3️⃣  TRUY CẬP:" -ForegroundColor White
Write-Host "   Admin Dashboard: $tunnelUrl" -ForegroundColor Cyan
Write-Host "   Test QR: $tunnelUrl/qr/POI_UA8AG0H2D" -ForegroundColor Cyan
Write-Host ""
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host ""

# Ask user to continue
$continue = Read-Host "Nhấn ENTER để kết nối tunnel (hoặc Ctrl+C để thoát)"

if ([string]::IsNullOrEmpty($continue)) {
    Write-Host ""
    Write-Host "🚀 Đang kết nối tunnel..." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "⚠️  GHI CHÚ QUAN TRỌNG:" -ForegroundColor Red
    Write-Host "   - Server phải đang chạy trên port $serverPort" -ForegroundColor Yellow
    Write-Host "   - Giữ cửa sổ này mở (không đóng)" -ForegroundColor Yellow
    Write-Host "   - Nhấn Ctrl+C để dừng tunnel" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "=" * 60 -ForegroundColor Cyan
    Write-Host "🌐 PUBLIC URL: $tunnelUrl" -ForegroundColor Green -BackgroundColor DarkGreen
    Write-Host "=" * 60 -ForegroundColor Cyan
    Write-Host ""
    
    # Host the tunnel
    devtunnel host $tunnel.tunnelId
}
