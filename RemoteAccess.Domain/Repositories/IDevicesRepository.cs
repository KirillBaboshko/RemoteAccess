using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Domain.Repositories
{
    /// <summary>
    /// Репозиторий для работы с девайсами
    /// </summary>
    public interface IDevicesRepository
    {
        /// <summary>
        /// Создание (агрегат)
        /// </summary>
        /// <param name="entity"><see cref="Device"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        Task CreateDevice(Device entity, CancellationToken cancellationToken);

        /// <summary>
        /// Получение (агрегат)
        /// </summary>
        /// <param name="ids"><see cref="IEnumerable{DeviceId}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Device"/></returns>
        Task<List<Device>> GetDeviceByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken);

        /// <summary>
        /// Получение (агрегат)
        /// </summary>
        /// <param name="id"><see cref="DeviceId"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Device"/></returns>
        Task<Device> GetDeviceById(Guid id, CancellationToken cancellationToken);
    }

}
