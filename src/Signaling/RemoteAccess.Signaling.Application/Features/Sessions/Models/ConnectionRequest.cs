namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public record ConnectionRequest(Guid SourceDeviceId, Guid TargetDeviceId);
