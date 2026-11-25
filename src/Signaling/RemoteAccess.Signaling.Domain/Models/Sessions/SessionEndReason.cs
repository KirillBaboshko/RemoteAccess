namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public enum SessionEndReason
{
    Rejected,
    ParticipantEnded,
    ConnectionLost,
    Timeout,
    NegotiationTimeout,
    ConnectionTimeout,
    WebRtcFailed,
    WebRtcClosed,
    RecoveryTimeout
}
