using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с девайсами
/// </summary>
public interface IDevicesRepository
{
    /// <summary>
    /// Создание устройства
    /// </summary>
    /// <param name="entity"><see cref="Device"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    Task CreateDevice(Device entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получение устройства по идентификатору
    /// </summary>
    /// <param name="id"><see cref="DeviceId"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Device"/></returns>
    Task<Device?> GetDeviceById(DeviceId id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение устройств по списку идентификаторов
    /// </summary>
    /// <param name="ids"><see cref="IEnumerable{DeviceId}"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="List{Device}"/></returns>
    Task<List<Device>> GetDeviceByIds(IEnumerable<DeviceId> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление устройства
    /// </summary>
    /// <param name="device"><see cref="Device"/></param>
    void Delete(Device device);

    /// <summary>
    /// Проверка существования устройства
    /// </summary>
    /// <param name="id"><see cref="DeviceId"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>True если существует</returns>
    Task<bool> ExistsAsync(DeviceId id, CancellationToken cancellationToken);
}
