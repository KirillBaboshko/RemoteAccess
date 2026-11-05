using RemoteAccess.Common.Domain;

namespace RemoteAccess.Domain.Models.Devices;

/// <summary>
/// Strongly-typed идентификатор устройства
/// </summary>
public sealed record DeviceId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    /// <summary>
    /// Создание нового идентификатора
    /// </summary>
    public static DeviceId New() => new(Guid.NewGuid());

    /// <summary>
    /// Создание из строки
    /// </summary>
    public static DeviceId FromString(string value) => new(Guid.Parse(value));
}
