using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Models.WebRtc;

namespace RemoteAccess.Signaling.Clients.Signaling.Models;

public record SessionModel(Guid SessionId, Guid SourceUserId, Guid SourceDeviceId,
    Guid TargetDeviceId, SessionState State, DateTime ExpiresAt, long Version = 1,
    WebRtcConnectionState SourceConnectionState = WebRtcConnectionState.New,
    WebRtcConnectionState TargetConnectionState = WebRtcConnectionState.New,
    DateTime? ConnectionExpiresAt = null, DateTime? RecoveryExpiresAt = null,
    DateTime? ConnectedAt = null, SessionEndReason? EndReason = null);
