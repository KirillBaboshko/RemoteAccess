using RemoteAccess.Signaling.Domain.Exceptions;

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

    public RemoteSession(SessionParticipant source, SessionParticipant target, DateTime expiresAt)
    {
        Source = source;
        Target = target;
        ExpiresAt = expiresAt;
    }

    public RemoteSession Respond(string connectionId, bool accepted, DateTime now)
    {
        if (Target.ConnectionId != connectionId || State != SessionState.Pending || ExpiresAt <= now)
            throw new SignalingException(ValidationErrorMessages.RequestUnavailable);

        return this with
        {
            State = accepted ? SessionState.Accepted : SessionState.Rejected,
            AcceptedAt = accepted ? now : null,
            EndedAt = accepted ? null : now,
            EndReason = accepted ? null : SessionEndReason.Rejected,
            Version = Version + 1
        };
    }

    public RemoteSession End(string connectionId)
    {
        if (Source.ConnectionId != connectionId && Target.ConnectionId != connectionId)
            throw new SignalingException(ValidationErrorMessages.SessionUnavailable);

        return this with { State = SessionState.Ended, EndedAt = DateTime.UtcNow,
            EndReason = SessionEndReason.ParticipantEnded, Version = Version + 1 };
    }

    public RemoteSession Expire() => this with { State = SessionState.Expired, EndedAt = DateTime.UtcNow,
        EndReason = SessionEndReason.Timeout, Version = Version + 1 };

    public static RemoteSession Restore(Guid id, SessionParticipant source, SessionParticipant target,
        DateTime expiresAt, SessionState state, DateTime createdAt, DateTime? acceptedAt,
        DateTime? endedAt, SessionEndReason? endReason, long version) => new(source, target, expiresAt)
        {
            Id = id, State = state, CreatedAt = createdAt, AcceptedAt = acceptedAt,
            EndedAt = endedAt, EndReason = endReason, Version = version
        };
}
