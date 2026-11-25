namespace RemoteAccess.Signaling.Domain.Models.Sessions;

public enum NegotiationState
{
    None,
    AwaitingOffer,
    AwaitingAnswer,
    Completed
}
