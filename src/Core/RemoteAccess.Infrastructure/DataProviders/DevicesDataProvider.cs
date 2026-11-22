using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Data.DataProviders.Devices.Model;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Infrastructure.Database;

namespace RemoteAccess.Infrastructure.DataProviders;

/// <summary>
/// Провайдер данных для чтения информации об устройствах
/// </summary>
public class DevicesDataProvider : IDeviceDataProvider
{
    private readonly DbSet<Device> _devices;
    private readonly IMapper _mapper;

    public DevicesDataProvider(AppDbContext dbContext, IMapper mapper)
    {
        _devices = dbContext.Set<Device>();
        _mapper = mapper;
    }

    public async Task<List<DeviceModel>> GetDevicesByOwnerId(Guid ownerId, CancellationToken cancellationToken)
    {
        var userId = new UserId(ownerId);
        var devices = await _devices
            .AsNoTracking()
            .Where(device => device.OwnerId == userId)
            .OrderBy(device => device.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DeviceModel>>(devices);
    }

    public async Task<List<DeviceModel>> SearchDevices(string search, CancellationToken cancellationToken)
    {
        var isDeviceId = Guid.TryParse(search, out var id);
        var deviceId = new DeviceId(id);
        var name = search.ToLower();
        var devices = await _devices
            .AsNoTracking()
            .Where(device => (isDeviceId && device.Id == deviceId) || device.Name.ToLower().Contains(name))
            .OrderBy(device => device.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DeviceModel>>(devices);
    }

    public async Task<DeviceAccessModel?> GetDeviceAccess(Guid deviceId, Guid userId, CancellationToken cancellationToken)
    {
        var id = new DeviceId(deviceId);
        var device = await _devices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        return device is null ? null : new DeviceAccessModel(device.OwnerId.Value == userId, device.Name);
    }
}
