using SQLite;
using DoAnCSharp.AdminWeb.Models;

namespace DoAnCSharp.AdminWeb.Services;

public class DatabaseService
{
    private const string DbFileName = "VinhKhanhTour_Full.db3";
    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task InitAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_connection != null) return;

            string dbPath;
            
            // Check for custom database path from environment variable
            var customPath = Environment.GetEnvironmentVariable("VINHKHANH_DB_PATH");
            if (!string.IsNullOrEmpty(customPath) && Directory.Exists(customPath))
            {
                dbPath = Path.Combine(customPath, DbFileName);
            }
            else
            {
                // Default path - same as MAUI app
                var appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "VinhKhanhTour"
                );
                Directory.CreateDirectory(appDataPath);
                dbPath = Path.Combine(appDataPath, DbFileName);
            }

            _connection = new SQLiteAsyncConnection(dbPath);

            // Create tables
            await _connection.CreateTableAsync<AudioPOI>();
            await _connection.CreateTableAsync<User>();
            await _connection.CreateTableAsync<PlayHistory>();
            await _connection.CreateTableAsync<UserPayment>();
            await _connection.CreateTableAsync<QRScanLimit>();
            await _connection.CreateTableAsync<UserStatus>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // POI Operations
    public async Task<List<AudioPOI>> GetAllPOIsAsync()
    {
        await InitAsync();
        return await _connection!.Table<AudioPOI>().ToListAsync();
    }

    public async Task<AudioPOI?> GetPOIByIdAsync(int id)
    {
        await InitAsync();
        return await _connection!.Table<AudioPOI>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> InsertPOIAsync(AudioPOI poi)
    {
        await InitAsync();
        return await _connection!.InsertAsync(poi);
    }

    public async Task<int> UpdatePOIAsync(AudioPOI poi)
    {
        await InitAsync();
        return await _connection!.UpdateAsync(poi);
    }

    public async Task<int> DeletePOIAsync(int id)
    {
        await InitAsync();
        return await _connection!.DeleteAsync<AudioPOI>(id);
    }

    // User Operations
    public async Task<List<User>> GetAllUsersAsync()
    {
        await InitAsync();
        return await _connection!.Table<User>().ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        await InitAsync();
        return await _connection!.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> InsertUserAsync(User user)
    {
        await InitAsync();
        return await _connection!.InsertAsync(user);
    }

    public async Task<int> UpdateUserAsync(User user)
    {
        await InitAsync();
        return await _connection!.UpdateAsync(user);
    }

    public async Task<int> DeleteUserAsync(int id)
    {
        await InitAsync();
        return await _connection!.DeleteAsync<User>(id);
    }

    // Play History Operations
    public async Task<List<PlayHistory>> GetAllHistoryAsync()
    {
        await InitAsync();
        return await _connection!.Table<PlayHistory>().OrderByDescending(h => h.PlayedAt).ToListAsync();
    }

    public async Task<int> DeleteHistoryAsync(int id)
    {
        await InitAsync();
        return await _connection!.DeleteAsync<PlayHistory>(id);
    }

    // User Payment Operations
    public async Task<UserPayment?> GetUserPaymentByUserIdAsync(int userId)
    {
        await InitAsync();
        return await _connection!.Table<UserPayment>().Where(p => p.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<int> UpdateUserPaymentAsync(UserPayment payment)
    {
        await InitAsync();
        return await _connection!.UpdateAsync(payment);
    }

    public async Task<int> InsertUserPaymentAsync(UserPayment payment)
    {
        await InitAsync();
        return await _connection!.InsertAsync(payment);
    }

    // QR Scan Limit Operations
    public async Task<QRScanLimit?> GetQRScanLimitByUserIdAsync(int userId)
    {
        await InitAsync();
        var limit = await _connection!.Table<QRScanLimit>().Where(q => q.UserId == userId).FirstOrDefaultAsync();

        if (limit != null)
        {
            // Reset count if it's a new day
            if (limit.LastResetDate.Date < DateTime.Now.Date)
            {
                limit.ScanCount = 0;
                limit.LastResetDate = DateTime.Now;
                await _connection!.UpdateAsync(limit);
            }
        }
        return limit;
    }

    public async Task<bool> CanUserScanQRAsync(int userId, bool isPaidUser)
    {
        await InitAsync();
        var limit = await GetQRScanLimitByUserIdAsync(userId);

        if (limit == null)
        {
            limit = new QRScanLimit
            {
                UserId = userId,
                IsPaidUser = isPaidUser,
                MaxScans = isPaidUser ? int.MaxValue : 5,
                ScanCount = 0,
                LastResetDate = DateTime.Now
            };
            await InsertQRScanLimitAsync(limit);
            return true;
        }

        if (isPaidUser) return true; // Paid users: unlimited
        return limit.ScanCount < limit.MaxScans; // Free users: limited
    }

    public async Task IncrementQRScanCountAsync(int userId)
    {
        await InitAsync();
        var limit = await GetQRScanLimitByUserIdAsync(userId);
        if (limit != null)
        {
            limit.ScanCount++;
            await _connection!.UpdateAsync(limit);
        }
    }

    public async Task<int> InsertQRScanLimitAsync(QRScanLimit limit)
    {
        await InitAsync();
        return await _connection!.InsertAsync(limit);
    }

    public async Task<int> UpdateQRScanLimitAsync(QRScanLimit limit)
    {
        await InitAsync();
        return await _connection!.UpdateAsync(limit);
    }

    // User Status Operations
    public async Task<UserStatus?> GetUserStatusAsync(int userId)
    {
        await InitAsync();
        return await _connection!.Table<UserStatus>().Where(s => s.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<List<UserStatus>> GetAllUserStatusAsync()
    {
        await InitAsync();
        return await _connection!.Table<UserStatus>().ToListAsync();
    }

    public async Task<int> UpdateUserStatusAsync(UserStatus status)
    {
        await InitAsync();
        return await _connection!.UpdateAsync(status);
    }

    public async Task<int> InsertUserStatusAsync(UserStatus status)
    {
        await InitAsync();
        return await _connection!.InsertAsync(status);
    }

    public async Task SetUserOnlineAsync(int userId, bool isOnline, string deviceInfo = "", string ipAddress = "")
    {
        await InitAsync();
        var status = await GetUserStatusAsync(userId);

        if (status == null)
        {
            status = new UserStatus
            {
                UserId = userId,
                IsOnline = isOnline,
                LastActiveAt = DateTime.Now,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress
            };
            await InsertUserStatusAsync(status);
        }
        else
        {
            status.IsOnline = isOnline;
            status.LastActiveAt = DateTime.Now;
            if (!string.IsNullOrEmpty(deviceInfo)) status.DeviceInfo = deviceInfo;
            if (!string.IsNullOrEmpty(ipAddress)) status.IpAddress = ipAddress;
            await UpdateUserStatusAsync(status);
        }
    }
}