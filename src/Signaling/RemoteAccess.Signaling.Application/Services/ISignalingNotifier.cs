using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Application.Services;

public interface ISignalingNotifier
{
    Task IncomingRequest(RemoteSession session);
    Task SessionUpdated(RemoteSession session);
}
