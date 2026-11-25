using AutoMapper;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public sealed class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<RemoteSession, SessionModel>().ConstructUsing(session => new SessionModel(
            session.Id, session.Source.UserId, session.Source.DeviceId, session.Target.DeviceId,
            session.State, session.ExpiresAt, session.Version,
            session.SourceConnectionState, session.TargetConnectionState,
            session.ConnectionExpiresAt, session.RecoveryExpiresAt, session.ConnectedAt, session.EndReason));
    }
}
