
#nullable disable

using SQLite;
using DoAnCSharp.Models;
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace DoAnCSharp.Services;

public interface IAuthService
{
    Task SetLoggedInAsync(bool status);
    Task<bool> IsLoggedInAsync();
    void Logout();
    Task<bool> RegisterAsync(string email, string password, string fullName, string avatar = "dotnet_bot.png");
}

public class AuthService : IAuthService
{
    private const string LoginKey = "isLoggedIn";
    private SQLiteAsyncConnection _connection;

    private async Task InitAsync()
    {
        if (_connection != null)
            return;

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "FoodTourDB.db3");
        _connection = new SQLiteAsyncConnection(dbPath);
        await _connection.CreateTableAsync<User>();
    }

    public Task SetLoggedInAsync(bool status)
    {
        Preferences.Default.Set(LoginKey, status);
        return Task.CompletedTask;
    }

    public Task<bool> IsLoggedInAsync()
    {
        bool status = Preferences.Default.Get(LoginKey, false);
        return Task.FromResult(status);
    }

    public void Logout()
    {
        Preferences.Default.Remove(LoginKey);
        Preferences.Default.Remove("CurrentUserEmail");
    }

    public async Task<bool> RegisterAsync(string email, string password, string fullName, string avatar = "dotnet_bot.png")
    {
        await InitAsync();

        var existingUser = await _connection.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        if (existingUser != null)
            return false;

        var newUser = new User
        {
            Email = email,
            Password = password,
            FullName = fullName,
            Avatar = avatar
        };

        await _connection.InsertAsync(newUser);
        return true;
    }
}