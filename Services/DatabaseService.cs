using SQLite;
using DoAnCSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace DoAnCSharp.Services;

public class DatabaseService
{
    private const string DbFileName = "VinhKhanhTour.db3";
    private SQLiteAsyncConnection? _connection;

    // Hàm khởi tạo CSDL
    private async Task InitAsync()
    {
        if (_connection != null) return; // Đã khởi tạo rồi thì bỏ qua

        // Tìm đường dẫn an toàn để lưu file trên điện thoại (Android/iOS)
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DbFileName);
        _connection = new SQLiteAsyncConnection(dbPath);

        // Tạo bảng AudioPOI trong file DB
        await _connection.CreateTableAsync<AudioPOI>();

        // [SEED DATA] Nếu bảng trống (mở app lần đầu), tự động thêm 3 quán ốc mẫu vào
        var count = await _connection.Table<AudioPOI>().CountAsync();
        if (count == 0)
        {
            var initialData = new List<AudioPOI>
            {
                new AudioPOI { Name = "Ốc Oanh", Description = "Chào mừng đến Ốc Oanh. Món chính là ốc hương xào bắp bơ.", Lat = 10.7600, Lng = 106.7000, Radius = 50, Priority = 1, ImageAsset = "oc_oanh.jpg" },
                new AudioPOI { Name = "Ốc Đào 2", Description = "Ốc Đào nổi tiếng với các món ốc xào me đậm đà.", Lat = 10.7581, Lng = 106.7061, Radius = 50, Priority = 1, ImageAsset = "oc_dao2.webp" },
                new AudioPOI { Name = "Ốc Vũ", Description = "Ốc Vũ là điểm đến lý tưởng cho món ốc mỡ xào tỏi.", Lat = 10.7578, Lng = 106.7058, Radius = 50, Priority = 2, ImageAsset = "dotnet_bot.png" }
            };
            // Chèn 1 lúc cả danh sách vào DB
            await _connection.InsertAllAsync(initialData);
        }
    }

    // Hàm gọi danh sách quán ốc ra ngoài để MapPage sử dụng
    public async Task<List<AudioPOI>> GetPOIsAsync()
    {
        await InitAsync();
        return await _connection!.Table<AudioPOI>().ToListAsync();
    }
}