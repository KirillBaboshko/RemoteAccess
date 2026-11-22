using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Api.Hubs;

public interface ISignalingClient
{
    Task IncomingRequest(SessionModel session);
    Task SessionUpdated(SessionModel session);
}
