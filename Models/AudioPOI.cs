using SQLite;

namespace DoAnCSharp.Models;

public class AudioPOI
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public int Radius { get; set; } = 50;
    public int Priority { get; set; } = 1;
    public string ImageAsset { get; set; } = "dotnet_bot.png";

    // 🔌 QR Code để quét và lấy audio thuyết minh
    public string? QRCode { get; set; }

    // 🗣️ Link audio thuyết minh (nếu có)
    public string? AudioUrl { get; set; }

    // 👤 ID chủ quán (nếu được add từ web)
    public int? OwnerId { get; set; }

    // 📅 Ngày tạo
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // 📝 Ngày chỉnh sửa cuối
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Ignore]
    public string DistanceInfo { get; set; } = "";
}