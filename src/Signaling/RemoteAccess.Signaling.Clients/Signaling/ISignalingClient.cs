using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Clients.Signaling;

public interface ISignalingClient
{
    Task IncomingRequest(SessionModel session);
    Task SessionUpdated(SessionModel session);
    Task OfferReceived(SessionDescriptionMessage offer);
    Task AnswerReceived(SessionDescriptionMessage answer);
    Task IceCandidateReceived(IceCandidateMessage candidate);
}
