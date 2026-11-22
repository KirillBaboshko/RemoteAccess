using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Infrastructure.Redis.Models;

public sealed record RedisSessionRecord(Guid Id, SessionParticipant Source, SessionParticipant Target,
    long ExpiresAt, SessionState State, long CreatedAt, long? AcceptedAt, long? EndedAt,
    SessionEndReason? EndReason, long Version)
{
    public RemoteSession ToDomain() => RemoteSession.Restore(Id, Source, Target,
        DateTimeOffset.FromUnixTimeMilliseconds(ExpiresAt).UtcDateTime, State,
        DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt).UtcDateTime,
        AcceptedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(AcceptedAt.Value).UtcDateTime : null,
        EndedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(EndedAt.Value).UtcDateTime : null,
        EndReason, Version);
}
