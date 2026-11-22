using RemoteAccess.Common.Domain;

namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public sealed class SessionHistory : Entity<SessionId, Guid>
{
    public Guid SourceUserId { get; private set; }
    public Guid SourceDeviceId { get; private set; }
    public Guid TargetUserId { get; private set; }
    public Guid TargetDeviceId { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public SessionState State { get; private set; }
    public SessionEndReason? EndReason { get; private set; }
    public long Version { get; private set; }

    private SessionHistory() { }

    public SessionHistory(RemoteSession session) => Apply(session);

    public void Apply(RemoteSession session)
    {
        if (session.Version <= Version) return;
        Id = new SessionId(session.Id);
        SourceUserId = session.Source.UserId;
        SourceDeviceId = session.Source.DeviceId;
        TargetUserId = session.Target.UserId;
        TargetDeviceId = session.Target.DeviceId;
        CreatedAt = session.CreatedAt;
        AcceptedAt = session.AcceptedAt;
        EndedAt = session.EndedAt;
        State = session.State;
        EndReason = session.EndReason;
        Version = session.Version;
        UpdatedAt = session.EndedAt ?? session.AcceptedAt;
    }
}
