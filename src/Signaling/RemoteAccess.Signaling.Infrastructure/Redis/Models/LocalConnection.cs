namespace RemoteAccess.Signaling.Infrastructure.Redis.Models;

public sealed record LocalConnection(Guid UserId, string ConnectionId, string HostDeviceId = "");
