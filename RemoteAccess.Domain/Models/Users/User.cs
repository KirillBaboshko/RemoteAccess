namespace RemoteAccess.Domain.Models.Users;

using Microsoft.AspNetCore.Identity;
using RemoteAccess.Domain.Models.Devices;

/// <summary>
/// Пользователь
/// </summary>
public class User: IdentityUser<Guid>
{

    public UserEmail UserEmail => new UserEmail(Email ?? string.Empty, IsEmailConfirmed);
    /// <summary>
    /// Соль пароля
    /// </summary>
    public string PasswordSalt { get; private set; }

    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Email подтвержден
    /// </summary>
    public bool IsEmailConfirmed { get; private set; }
    public IReadOnlyCollection<Device> Devices => _devices;
    private readonly List<Device> _devices;

    /// <summary>
    /// Конструктор для EF Core
    /// </summary>
    private User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        PasswordSalt = string.Empty;
        _devices = new();
    }

    /// <summary>
    /// Конструктор для создания пользователя
    /// </summary>
    public User(string email, string password)
    {
        Id = Guid.NewGuid();
        Email = email.ToLowerInvariant();
        PasswordSalt = GenerateSalt();
        PasswordHash = HashPassword(password, PasswordSalt);
        _devices = new();
        CreatedAt = DateTime.UtcNow;
        IsEmailConfirmed = false;
    }
    public User(UserEmail emailUser, string password)
    {
        Id = Guid.NewGuid();
        Email = emailUser.Email.ToLowerInvariant();
        PasswordSalt = GenerateSalt();
        PasswordHash = HashPassword(password, PasswordSalt);
        _devices = new();
        CreatedAt = DateTime.UtcNow;
        IsEmailConfirmed = emailUser.Confirmed;
    }
    /// <summary>
    /// Проверка пароля
    /// </summary>
    public bool CheckPassword(string password)
    {
        var hash = HashPassword(password, PasswordSalt);
        return PasswordHash == hash;
    }

    /// <summary>
    /// Подтверждение email
    /// </summary>
    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
    }
    public void AddDevice(Device device)
    {
        if (device.OwnerId != Id)
        {
            throw new InvalidOperationException("Device owner does not match user.");
        }
        _devices.Add(device);
    }
    public void RemoveDevice(Device device)
    {
        if (device.OwnerId != Id)
        {
            throw new InvalidOperationException("Device owner does not match user.");
        }
        _devices.Remove(device);
    }

    private static string GenerateSalt()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var combined = password + salt;
        var bytes = System.Text.Encoding.UTF8.GetBytes(combined);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
