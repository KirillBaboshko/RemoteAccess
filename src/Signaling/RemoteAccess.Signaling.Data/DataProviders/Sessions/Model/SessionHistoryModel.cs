using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

public sealed class SessionHistoryModel
{
    public Guid SessionId { get; init; }
    public Guid SourceUserId { get; init; }
    public Guid TargetUserId { get; init; }
    public Guid SourceDeviceId { get; init; }
    public Guid TargetDeviceId { get; init; }
    public SessionState State { get; init; }
    public SessionEndReason? EndReason { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? AcceptedAt { get; init; }
    public DateTime? ConnectedAt { get; init; }
    public DateTime? EndedAt { get; init; }
    public double? DurationSeconds => ConnectedAt.HasValue && EndedAt.HasValue
        ? (EndedAt.Value - ConnectedAt.Value).TotalSeconds
        : null;
}
