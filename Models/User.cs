using SQLite;

namespace DoAnCSharp.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    [Unique] 
    public string Email { get; set; } = string.Empty;

    // THÊM DÒNG NÀY ĐỂ LƯU MẬT KHẨU
    public string Password { get; set; } = string.Empty;

    public string Avatar { get; set; } = "dotnet_bot.png"; 
    public string Phone { get; set; } = string.Empty;

    // 🔌 Thêm trường ngôn ngữ để đồng bộ giữa app và web admin
    public string Language { get; set; } = "vi";

    // 💳 Thêm trường thanh toán - true = đã thanh toán, false = miễn phí
    public bool IsPaid { get; set; } = false;
}
