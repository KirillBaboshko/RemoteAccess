using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RemoteAccess.Signaling.Api.Hubs;
using RemoteAccess.Signaling.Application.Services;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Api.Services;

public sealed class SignalingNotifier : ISignalingNotifier
{
    private readonly IHubContext<SignalingHub, ISignalingClient> _hub;
    private readonly IMapper _mapper;

    public SignalingNotifier(IHubContext<SignalingHub, ISignalingClient> hub, IMapper mapper)
    {
        _hub = hub;
        _mapper = mapper;
    }

    public Task IncomingRequest(RemoteSession session) =>
        _hub.Clients.Client(session.Target.ConnectionId).IncomingRequest(_mapper.Map<SessionModel>(session));

    public Task SessionUpdated(RemoteSession session)
    {
        var model = _mapper.Map<SessionModel>(session);
        return Task.WhenAll(
            _hub.Clients.Client(session.Source.ConnectionId).SessionUpdated(model),
            _hub.Clients.Client(session.Target.ConnectionId).SessionUpdated(model));
    }
}
