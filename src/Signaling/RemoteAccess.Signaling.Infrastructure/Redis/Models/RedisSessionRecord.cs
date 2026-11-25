using FluentValidation;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Models.WebRtc;

namespace RemoteAccess.Signaling.Infrastructure.Redis.Models;

public sealed record RedisSessionRecord(Guid Id, SessionParticipant Source, SessionParticipant Target,
    long ExpiresAt, SessionState State, long CreatedAt, long? AcceptedAt, long? EndedAt,
    SessionEndReason? EndReason, long Version,
    NegotiationState NegotiationState = NegotiationState.None, long? NegotiationExpiresAt = null,
    WebRtcConnectionState SourceConnectionState = WebRtcConnectionState.New,
    WebRtcConnectionState TargetConnectionState = WebRtcConnectionState.New,
    long? ConnectionExpiresAt = null, long? RecoveryExpiresAt = null,
    long? ConnectedAt = null, long? UpdatedAt = null)
{
    public RemoteSession ToDomain()
    {
        RedisSessionRecordValidator.Default.ValidateAndThrow(this);
        return RemoteSession.Restore(Id, Source, Target,
            DateTimeOffset.FromUnixTimeMilliseconds(ExpiresAt).UtcDateTime, State,
            DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt).UtcDateTime,
            AcceptedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(AcceptedAt.Value).UtcDateTime : null,
            EndedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(EndedAt.Value).UtcDateTime : null,
            EndReason, Version, NegotiationState,
            NegotiationExpiresAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(NegotiationExpiresAt.Value).UtcDateTime : null,
            SourceConnectionState, TargetConnectionState,
            ConnectionExpiresAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(ConnectionExpiresAt.Value).UtcDateTime : null,
            RecoveryExpiresAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(RecoveryExpiresAt.Value).UtcDateTime : null,
            ConnectedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(ConnectedAt.Value).UtcDateTime : null,
            UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt.Value).UtcDateTime : null);
    }
}
