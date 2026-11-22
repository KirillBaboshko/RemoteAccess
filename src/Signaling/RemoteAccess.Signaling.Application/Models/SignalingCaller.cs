namespace RemoteAccess.Signaling.Application.Models;

public record SignalingCaller(Guid UserId, string ConnectionId, string AccessToken);
