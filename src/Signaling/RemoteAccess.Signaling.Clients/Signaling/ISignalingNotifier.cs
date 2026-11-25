using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Clients.Signaling;

public interface ISignalingNotifier
{
    Task IncomingRequest(RemoteSession session);
    Task SessionUpdated(RemoteSession session);
    Task OfferReceived(string connectionId, SessionDescriptionMessage offer);
    Task AnswerReceived(string connectionId, SessionDescriptionMessage answer);
    Task IceCandidateReceived(string connectionId, IceCandidateMessage candidate);
}
