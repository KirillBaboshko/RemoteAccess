namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public record SessionParticipant(Guid UserId, Guid DeviceId, string ConnectionId);
