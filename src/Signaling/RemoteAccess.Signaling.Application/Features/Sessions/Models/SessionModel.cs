using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public record SessionModel(Guid SessionId, Guid SourceUserId, Guid SourceDeviceId,
    Guid TargetDeviceId, SessionState State, DateTime ExpiresAt);
