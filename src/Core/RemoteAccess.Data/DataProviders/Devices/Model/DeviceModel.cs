using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Data.DataProviders.Devices.Model;

/// <summary>
/// Выходная модель устройства
/// </summary>
public class DeviceModel
{
    /// <summary>
    /// Идентификатор устройства
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название устройства
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Тип платформы устройства
    /// </summary>
    public PlatformType PlatformType { get; init; }

    /// <summary>
    /// Статус устройства
    /// </summary>
    public DeviceStatus Status { get; set; }

    /// <summary>
    /// Можно ли подключиться к устройству
    /// </summary>
    public bool IsCanConnected => Status == DeviceStatus.Online;

    /// <summary>
    /// Дата создания устройства
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
}
