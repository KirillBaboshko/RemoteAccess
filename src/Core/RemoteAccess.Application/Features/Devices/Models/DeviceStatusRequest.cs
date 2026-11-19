namespace RemoteAccess.Application.Features.Devices.Models;

public record DeviceStatusRequest(Guid OwnerId, Guid DeviceId);
