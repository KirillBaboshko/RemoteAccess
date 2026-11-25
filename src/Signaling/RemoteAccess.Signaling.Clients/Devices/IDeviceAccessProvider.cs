using RemoteAccess.Signaling.Clients.Devices.Models;

namespace RemoteAccess.Signaling.Clients.Devices;

public interface IDeviceAccessProvider
{
    Task<DeviceAccessModel?> GetAccess(Guid deviceId, string accessToken, CancellationToken cancellationToken);
}
