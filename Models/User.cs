using SQLite;

namespace DoAnCSharp.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string FullName { get; set; } = string.Empty;
    
    [Unique] // Mỗi email chỉ được đăng ký 1 tài khoản
    public string Email { get; set; } = string.Empty;
    
    public string Avatar { get; set; } = "dotnet_bot.png"; // Ảnh mặc định
    
    public string Phone { get; set; } = string.Empty;
}