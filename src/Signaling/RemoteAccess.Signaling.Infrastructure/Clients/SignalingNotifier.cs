using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Infrastructure.Clients;

public sealed class SignalingNotifier<THub> : ISignalingNotifier where THub : Hub<ISignalingClient>
{
    private readonly IHubContext<THub, ISignalingClient> _hub;
    private readonly IMapper _mapper;

    public SignalingNotifier(IHubContext<THub, ISignalingClient> hub, IMapper mapper)
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

    public Task OfferReceived(string connectionId, SessionDescriptionMessage offer) =>
        _hub.Clients.Client(connectionId).OfferReceived(offer);

    public Task AnswerReceived(string connectionId, SessionDescriptionMessage answer) =>
        _hub.Clients.Client(connectionId).AnswerReceived(answer);

    public Task IceCandidateReceived(string connectionId, IceCandidateMessage candidate) =>
        _hub.Clients.Client(connectionId).IceCandidateReceived(candidate);
}
