using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Models.WebRtc;
using RemoteAccess.Signaling.Domain.Models.Devices;

namespace RemoteAccess.Signaling.Domain.Repositories;

public interface ISignalingState
{
    Task ConnectAsync(Guid userId, string connectionId, CancellationToken cancellationToken);
    Task RegisterHostAsync(SessionParticipant participant, CancellationToken cancellationToken);
    Task<RemoteSession> CreateSessionAsync(SessionParticipant source, Guid targetDeviceId, TimeSpan pendingTimeout, CancellationToken cancellationToken);
    Task<RemoteSession> RespondAsync(Guid sessionId, string connectionId, bool accepted, CancellationToken cancellationToken);
    Task<RemoteSession> EndAsync(Guid sessionId, string connectionId, CancellationToken cancellationToken);
    Task<RemoteSession> GetSessionAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken);
    Task<RemoteSession> PrepareWebRtcMessageAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcMessageType messageType, string? description, CancellationToken cancellationToken);
    Task<RemoteSession> ReportConnectionStateAsync(Guid sessionId, Guid userId, string connectionId,
        WebRtcConnectionState state, CancellationToken cancellationToken);
    Task<IReadOnlyList<RemoteSession>> DisconnectAsync(string connectionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RemoteSession>> ExpireSessionsAsync(CancellationToken cancellationToken);
    Task RenewLeasesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<DeviceAvailability>> GetDeviceAvailabilityAsync(Guid[] deviceIds, CancellationToken cancellationToken);
    Task UpdateDeviceStatusAsync(Guid deviceId, bool isOnline, CancellationToken cancellationToken);
}
