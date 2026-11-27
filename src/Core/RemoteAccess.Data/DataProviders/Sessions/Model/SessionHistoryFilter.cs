using RemoteAccess.Contracts.Signaling.Sessions;

namespace RemoteAccess.Data.DataProviders.Sessions.Model;

public sealed class SessionHistoryFilter
{
    public Guid? DeviceId { get; init; }
    public HistoryState? State { get; init; }
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
