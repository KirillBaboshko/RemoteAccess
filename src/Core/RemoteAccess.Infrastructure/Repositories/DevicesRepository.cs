using Microsoft.EntityFrameworkCore;
using RemoteAccess.Common.Data;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Infrastructure.Database;

namespace RemoteAccess.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с девайсами
/// </summary>
public class DevicesRepository : RepositoryBase<Device, DeviceId, Guid>, IDevicesRepository
{
    public DevicesRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Создание устройства
    /// </summary>
    public async Task CreateDevice(Device entity, CancellationToken cancellationToken)
    {
        InternalCreate(entity);
    }

    /// <summary>
    /// Получение устройства по идентификатору
    /// </summary>
    public async Task<Device?> GetDeviceById(DeviceId id, CancellationToken cancellationToken)
    {
        return await InternalGetByIdAsync(id, cancellationToken);
    }

    /// <summary>
    /// Получение устройств по списку идентификаторов
    /// </summary>
    public async Task<List<Device>> GetDeviceByIds(IEnumerable<DeviceId> ids, CancellationToken cancellationToken)
    {
        var idList = ids.ToList();
        return await Query
            .Where(d => idList.Contains(d.Id))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Удаление устройства
    /// </summary>
    public void Delete(Device device)
    {
        InternalDelete(device);
    }

    /// <summary>
    /// Проверка существования устройства
    /// </summary>
    public async Task<bool> ExistsAsync(DeviceId id, CancellationToken cancellationToken)
    {
        return await InternalExistsAsync(id, cancellationToken);
    }
}
