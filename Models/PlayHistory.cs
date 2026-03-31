using SQLite;
using System;

namespace DoAnCSharp.Models;

public class PlayHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    // ID của User đang nghe
    public int UserId { get; set; } 
    
    // ID của Quán ăn (POI)
    public int PoiId { get; set; }
    
    // Thời điểm phát gần nhất
    public DateTime LastPlayedAt { get; set; }
}