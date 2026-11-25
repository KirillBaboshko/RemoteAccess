namespace RemoteAccess.Signaling.Domain.Models.WebRtc;

public enum WebRtcConnectionState
{
    New,
    Connecting,
    Connected,
    Disconnected,
    Failed,
    Closed
}
