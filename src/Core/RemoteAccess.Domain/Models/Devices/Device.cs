using RemoteAccess.Common.Domain;
using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Domain.Models.Devices;

/// <summary>
/// Устройство для удаленного доступа
/// </summary>
public class Device : Entity<DeviceId, Guid>
{
    /// <summary>
    /// Идентификатор владельца устройства
    /// </summary>
    public UserId OwnerId { get; private set; } = default!;

    /// <summary>
    /// Название устройства
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Тип платформы устройства
    /// </summary>
    public PlatformType PlatformType { get; private set; }

    /// <summary>
    /// Статус устройства
    /// </summary>
    public DeviceStatus Status { get; private set; }

    /// <summary>
    /// Можно ли подключиться к устройству
    /// </summary>
    public bool IsCanConnected => Status == DeviceStatus.Online;

    /// <summary>
    /// Конструктор для EF Core
    /// </summary>
    private Device()
    {
    }

    /// <summary>
    /// Конструктор для создания устройства
    /// </summary>
    public Device(UserId ownerId, string name, PlatformType platformType)
    {
        Id = new DeviceId(Guid.NewGuid());
        OwnerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        PlatformType = platformType;
        Status = DeviceStatus.Offline;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Переключить статус устройства между Online и Offline
    /// </summary>
    public void ToggleStatus()
    {
        switch (Status)
        {
            case DeviceStatus.Online:
                Status = DeviceStatus.Offline;
                break;
            case DeviceStatus.Offline:
                Status = DeviceStatus.Online;
                break;
            default:
                return;
        }
        MarkAsUpdated();
    }

    /// <summary>
    /// Обновить название устройства
    /// </summary>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        MarkAsUpdated();
    }
}
