using SQLite;
using System;

namespace DoAnCSharp.Models;

public class PlayHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int UserId { get; set; }
    public string PoiName { get; set; } = string.Empty;
    public string POIName { get; set; } = string.Empty; // Alias for consistency
    public string ImageAsset { get; set; } = string.Empty;
    public DateTime PlayedAt { get; set; }
}