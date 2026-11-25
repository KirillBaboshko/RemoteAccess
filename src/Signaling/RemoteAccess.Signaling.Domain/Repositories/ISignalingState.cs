using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Models.WebRtc;

namespace RemoteAccess.Signaling.Domain.Repositories;

public interface ISignalingState
{
    Task ConnectAsync(Guid userId, string connectionId, CancellationToken cancellationToken);
    Task RegisterHostAsync(SessionParticipant participant, CancellationToken cancellationToken);
    Task<RemoteSession> CreateSessionAsync(SessionParticipant source, Guid targetDeviceId, TimeSpan pendingTimeout, CancellationToken cancellationToken);
    Task<RemoteSession> RespondAsync(Guid sessionId, string connectionId, bool accepted, CancellationToken cancellationToken);
    Task<RemoteSession> EndAsync(Guid sessionId, string connectionId, CancellationToken cancellationToken);
    Task<RemoteSession> PrepareWebRtcMessageAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcMessageType messageType, CancellationToken cancellationToken);
    Task<RemoteSession> ReportConnectionStateAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcConnectionState state, CancellationToken cancellationToken);
    Task<IReadOnlyList<RemoteSession>> DisconnectAsync(string connectionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RemoteSession>> ExpireSessionsAsync(CancellationToken cancellationToken);
    Task RenewLeasesAsync(CancellationToken cancellationToken);
}
