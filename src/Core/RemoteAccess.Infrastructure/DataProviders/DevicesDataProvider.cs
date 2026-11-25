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
    private readonly IDeviceAvailabilityProvider _availability;

    public DevicesDataProvider(AppDbContext dbContext, IMapper mapper, IDeviceAvailabilityProvider availability)
    {
        _devices = dbContext.Set<Device>();
        _mapper = mapper;
        _availability = availability;
    }

    public async Task<List<DeviceModel>> GetDevicesByOwnerId(Guid ownerId, CancellationToken cancellationToken)
    {
        var userId = new UserId(ownerId);
        var devices = await _devices
            .AsNoTracking()
            .Where(device => device.OwnerId == userId)
            .OrderBy(device => device.CreatedAt)
            .ToListAsync(cancellationToken);

        return await AddAvailability(_mapper.Map<List<DeviceModel>>(devices), cancellationToken);
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

        return await AddAvailability(_mapper.Map<List<DeviceModel>>(devices), cancellationToken);
    }

    public async Task<DeviceAccessModel?> GetDeviceAccess(Guid deviceId, Guid userId, CancellationToken cancellationToken)
    {
        var id = new DeviceId(deviceId);
        var device = await _devices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        return device is null ? null : new DeviceAccessModel(device.OwnerId.Value == userId, device.Name,
            device.Status == DeviceStatus.Online);
    }

    private async Task<List<DeviceModel>> AddAvailability(List<DeviceModel> devices, CancellationToken cancellationToken)
    {
        if (devices.Count == 0) return devices;
        var availability = await _availability.GetAvailability(devices.Select(device => device.Id).ToArray(), cancellationToken);
        foreach (var device in devices)
        {
            device.Status = (device.Status, availability[device.Id]) switch
            {
                (_, DeviceStatus.Busy) => DeviceStatus.Busy,
                (DeviceStatus.Online, DeviceStatus.Online) => DeviceStatus.Online,
                _ => DeviceStatus.Offline
            };
        }
        return devices;
    }
}
