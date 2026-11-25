using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Infrastructure.Redis;

namespace RemoteAccess.Signaling.Infrastructure.Clients;

public sealed class SignalingNotifier<THub> : ISignalingNotifier where THub : Hub<ISignalingClient>
{
    private readonly IHubContext<THub, ISignalingClient> _hub;
    private readonly IMapper _mapper;
    private readonly LocalConnections _local;

    public SignalingNotifier(IHubContext<THub, ISignalingClient> hub, IMapper mapper, LocalConnections local)
    {
        _hub = hub;
        _mapper = mapper;
        _local = local;
    }

    public Task IncomingRequest(RemoteSession session) =>
        _local.Contains(session.Target.ConnectionId)
            ? _hub.Clients.Client(session.Target.ConnectionId).IncomingRequest(_mapper.Map<SessionModel>(session))
            : Task.CompletedTask;

    public Task SessionUpdated(RemoteSession session)
    {
        var model = _mapper.Map<SessionModel>(session);
        var recipients = new[] { session.Source.ConnectionId, session.Target.ConnectionId }.Where(_local.Contains).ToArray();
        return recipients.Length == 0 ? Task.CompletedTask : _hub.Clients.Clients(recipients).SessionUpdated(model);
    }

    public Task OfferReceived(string connectionId, SessionDescriptionMessage offer) =>
        _hub.Clients.Client(connectionId).OfferReceived(offer);

    public Task AnswerReceived(string connectionId, SessionDescriptionMessage answer) =>
        _hub.Clients.Client(connectionId).AnswerReceived(answer);

    public Task IceCandidateReceived(string connectionId, IceCandidateMessage candidate) =>
        _hub.Clients.Client(connectionId).IceCandidateReceived(candidate);

    public Task DeviceAvailabilityChanged(DeviceAvailabilityChangedMessage change) =>
        _hub.Clients.Clients(_local.Snapshot()).DeviceAvailabilityChanged(change);
}
