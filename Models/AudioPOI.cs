using SQLite; // MỚI: Khai báo sử dụng SQLite

namespace DoAnCSharp.Models; // Chú ý đổi namespace cho đúng

public class AudioPOI
{
    // MỚI: Phải có khóa chính (ID) tự động tăng cho Cơ sở dữ liệu
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } 

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double Radius { get; set; }
    public int Priority { get; set; }
    public string ImageAsset { get; set; } = string.Empty;
}