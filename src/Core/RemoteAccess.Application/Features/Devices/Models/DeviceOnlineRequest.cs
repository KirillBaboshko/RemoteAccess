namespace RemoteAccess.Application.Features.Devices.Models;

public record DeviceOnlineRequest(Guid OwnerId, Guid DeviceId);
