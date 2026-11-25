namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public enum SessionState
{
    Pending,
    Accepted,
    Rejected,
    Ended,
    Expired,
    Connected,
    Reconnecting
}
