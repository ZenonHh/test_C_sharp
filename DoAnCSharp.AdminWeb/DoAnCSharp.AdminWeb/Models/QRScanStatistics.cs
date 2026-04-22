using SQLite;

namespace DoAnCSharp.AdminWeb.Models;

/// <summary>
/// Model lưu trữ thống kê quét QR - Giúp tracking lịch sử quét cho mỗi quán ăn
/// </summary>
[Table("qr_scan_statistics")]
public class QRScanStatistics
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// ID của quán ăn được quét (POI)
    /// </summary>
    [Indexed]
    public int POIId { get; set; }

    /// <summary>
    /// ID của thiết bị quét
    /// </summary>
    [Indexed]
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Tên quán ăn (cache để query nhanh)
    /// </summary>
    public string POIName { get; set; } = string.Empty;

    /// <summary>
    /// Tên thiết bị
    /// </summary>
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// Loại thiết bị (iOS/Android/Windows)
    /// </summary>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// IP address của thiết bị
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Thời gian quét
    /// </summary>
    [Indexed]
    public DateTime ScanTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ngày quét (dùng để lọc theo ngày, tuần, tháng)
    /// </summary>
    [Indexed]
    public DateTime ScanDate { get; set; } = DateTime.UtcNow.Date;

    /// <summary>
    /// Ghi chú thêm nếu cần
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái quét (thành công/thất bại/vượt giới hạn)
    /// </summary>
    public string Status { get; set; } = "success"; // success, failed, limit_exceeded
}

/// <summary>
/// DTO để trả về thống kê QR scan
/// </summary>
public class QRScanStatisticsDTO
{
    public int TotalScansToday { get; set; }
    public int TotalScansThisWeek { get; set; }
    public int TotalScansThisMonth { get; set; }
    public int UniquePOIsScannedToday { get; set; }
    public int UniquePOIsScannedThisWeek { get; set; }
    public List<POIScanInfo> TopScannedPOIs { get; set; } = new();
    public List<QRScanStatistics> RecentScans { get; set; } = new();
}

/// <summary>
/// Thông tin quán ăn được quét nhiều nhất
/// </summary>
public class POIScanInfo
{
    public int POIId { get; set; }
    public string POIName { get; set; } = string.Empty;
    public int ScanCount { get; set; }
    public int UniqueDevices { get; set; }
    public DateTime LastScannedTime { get; set; }
    public string Percentage { get; set; } = "0%";
}
