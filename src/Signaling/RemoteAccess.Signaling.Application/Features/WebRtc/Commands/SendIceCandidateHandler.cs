using MediatR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.WebRtc;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendIceCandidateHandler : IRequestHandler<SendIceCandidateCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;

    public SendIceCandidateHandler(ISignalingState state, ISignalingNotifier notifier)
    {
        _state = state;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(SendIceCandidateCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.PrepareWebRtcMessageAsync(request.Candidate.SessionId,
            request.Caller.UserId, request.Caller.ConnectionId, WebRtcMessageType.IceCandidate, cancellationToken);
        var recipient = session.Source.ConnectionId == request.Caller.ConnectionId
            ? session.Target.ConnectionId : session.Source.ConnectionId;
        await _notifier.IceCandidateReceived(recipient, request.Candidate);
        return Unit.Value;
    }
}
