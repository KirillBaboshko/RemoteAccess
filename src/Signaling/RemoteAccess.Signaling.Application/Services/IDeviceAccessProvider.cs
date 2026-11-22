using RemoteAccess.Signaling.Application.Features.Devices.Models;

namespace RemoteAccess.Signaling.Application.Services;

public interface IDeviceAccessProvider
{
    Task<DeviceAccessModel?> GetAccess(Guid deviceId, string accessToken, CancellationToken cancellationToken);
}
