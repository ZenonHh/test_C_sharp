using Microsoft.AspNetCore.Mvc;
using DoAnCSharp.AdminWeb.Models;
using DoAnCSharp.AdminWeb.Services;

namespace DoAnCSharp.AdminWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QRScanStatisticsController : ControllerBase
{
    private readonly DatabaseService _db;
    private readonly ILogger<QRScanStatisticsController> _logger;

    public QRScanStatisticsController(DatabaseService db, ILogger<QRScanStatisticsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Lưu thống kê quét QR - Gọi từ /qr/{code} endpoint khi quét thành công
    /// </summary>
    [HttpPost("save")]
    public async Task<IActionResult> SaveScan([FromBody] QRScanStatistics scan)
    {
        if (scan == null || scan.POIId <= 0)
        {
            return BadRequest(new { error = "Invalid scan data" });
        }

        try
        {
            scan.ScanTime = DateTime.UtcNow;
            scan.ScanDate = scan.ScanTime.Date;
            
            var result = await _db.SaveQRScanStatisticsAsync(scan);
            
            _logger.LogInformation($"QR Scan saved: POI {scan.POIId}, Device {scan.DeviceId}");
            
            return Ok(new { id = result, message = "Thống kê quét được lưu thành công" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving QR scan: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lưu thống kê quét" });
        }
    }

    /// <summary>
    /// Lấy tất cả thống kê quét QR
    /// </summary>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllScans([FromQuery] int days = 30)
    {
        try
        {
            var scans = await _db.GetAllQRScansAsync();
            var filtered = scans.Where(s => s.ScanDate >= DateTime.UtcNow.Date.AddDays(-days)).ToList();
            return Ok(filtered);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting QR scans: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy thống kê quét" });
        }
    }

    /// <summary>
    /// Lấy thống kê tổng hợp
    /// </summary>
    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            var stats = await _db.GetQRScanStatisticsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting statistics: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy thống kê" });
        }
    }

    /// <summary>
    /// Lấy top 10 quán được quét nhiều nhất
    /// </summary>
    [HttpGet("top-pois")]
    public async Task<IActionResult> GetTopPOIs([FromQuery] int days = 30, [FromQuery] int limit = 10)
    {
        try
        {
            var topPOIs = await _db.GetTopScannedPOIsAsync(days, limit);
            return Ok(topPOIs);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top POIs: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy top quán" });
        }
    }

    /// <summary>
    /// Lấy lịch sử quét của một quán
    /// </summary>
    [HttpGet("poi/{poiId}/history")]
    public async Task<IActionResult> GetPOIHistory(int poiId, [FromQuery] int days = 30)
    {
        try
        {
            var history = await _db.GetPOIScanHistoryAsync(poiId, days);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting POI history: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy lịch sử quét" });
        }
    }

    /// <summary>
    /// Lấy lịch sử quét của một thiết bị
    /// </summary>
    [HttpGet("device/{deviceId}/history")]
    public async Task<IActionResult> GetDeviceHistory(string deviceId, [FromQuery] int days = 30)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            return BadRequest(new { error = "Device ID không được để trống" });
        }

        try
        {
            var history = await _db.GetDeviceScanHistoryAsync(deviceId, days);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting device history: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy lịch sử thiết bị" });
        }
    }

    /// <summary>
    /// Lấy số lần quét hôm nay
    /// </summary>
    [HttpGet("today-count")]
    public async Task<IActionResult> GetTodayCount()
    {
        try
        {
            var count = await _db.GetTodayScansCountAsync();
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting today count: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy số quét hôm nay" });
        }
    }

    /// <summary>
    /// Lấy số quán được quét hôm nay
    /// </summary>
    [HttpGet("unique-pois-today")]
    public async Task<IActionResult> GetUniquePOIsToday()
    {
        try
        {
            var count = await _db.GetUniquePOIsScannedTodayAsync();
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting unique POIs today: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy số quán hôm nay" });
        }
    }

    /// <summary>
    /// Lấy số quán được quét tuần này
    /// </summary>
    [HttpGet("unique-pois-week")]
    public async Task<IActionResult> GetUniquePOIsWeek()
    {
        try
        {
            var count = await _db.GetUniquePOIsScannedThisWeekAsync();
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting unique POIs week: {ex.Message}");
            return StatusCode(500, new { error = "Lỗi khi lấy số quán tuần này" });
        }
    }
}
