using SQLite;

namespace DoAnCSharp.AdminWeb.Models;

public class PlayHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string PoiName { get; set; } = string.Empty;
    public string ImageAsset { get; set; } = string.Empty;
    public DateTime PlayedAt { get; set; }
}
