using RemoteAccess.Signaling.Domain.Models.WebRtc;

namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public sealed record RemoteSession
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public SessionParticipant Source { get; }
    public SessionParticipant Target { get; }
    public DateTime ExpiresAt { get; }
    public SessionState State { get; private init; } = SessionState.Pending;
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
    public DateTime? AcceptedAt { get; private init; }
    public DateTime? EndedAt { get; private init; }
    public SessionEndReason? EndReason { get; private init; }
    public long Version { get; private init; } = 1;
    public NegotiationState NegotiationState { get; private init; }
    public DateTime? NegotiationExpiresAt { get; private init; }
    public WebRtcConnectionState SourceConnectionState { get; private init; }
    public WebRtcConnectionState TargetConnectionState { get; private init; }
    public DateTime? ConnectionExpiresAt { get; private init; }
    public DateTime? RecoveryExpiresAt { get; private init; }
    public DateTime? ConnectedAt { get; private init; }
    public DateTime? UpdatedAt { get; private init; }

    public RemoteSession(SessionParticipant source, SessionParticipant target, DateTime expiresAt)
    {
        Source = source;
        Target = target;
        ExpiresAt = expiresAt;
    }

    public static RemoteSession Restore(Guid id, SessionParticipant source, SessionParticipant target,
        DateTime expiresAt, SessionState state, DateTime createdAt, DateTime? acceptedAt,
        DateTime? endedAt, SessionEndReason? endReason, long version,
        NegotiationState negotiationState = NegotiationState.None, DateTime? negotiationExpiresAt = null,
        WebRtcConnectionState sourceConnectionState = WebRtcConnectionState.New,
        WebRtcConnectionState targetConnectionState = WebRtcConnectionState.New,
        DateTime? connectionExpiresAt = null, DateTime? recoveryExpiresAt = null,
        DateTime? connectedAt = null, DateTime? updatedAt = null) => new(source, target, expiresAt)
        {
            Id = id, State = state, CreatedAt = createdAt, AcceptedAt = acceptedAt,
            EndedAt = endedAt, EndReason = endReason, Version = version,
            NegotiationState = negotiationState, NegotiationExpiresAt = negotiationExpiresAt,
            SourceConnectionState = sourceConnectionState, TargetConnectionState = targetConnectionState,
            ConnectionExpiresAt = connectionExpiresAt, RecoveryExpiresAt = recoveryExpiresAt,
            ConnectedAt = connectedAt, UpdatedAt = updatedAt
        };
}
