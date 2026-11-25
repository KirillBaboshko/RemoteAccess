using MediatR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.WebRtc;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendOfferHandler : IRequestHandler<SendOfferCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;

    public SendOfferHandler(ISignalingState state, ISignalingNotifier notifier)
    {
        _state = state;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(SendOfferCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.PrepareWebRtcMessageAsync(request.Offer.SessionId,
            request.Caller.UserId, request.Caller.ConnectionId, WebRtcMessageType.Offer, request.Offer.Sdp, cancellationToken);
        var recipient = session.Target.ConnectionId;
        if (session.NegotiationState == NegotiationState.AwaitingAnswer)
            await _notifier.OfferReceived(recipient, request.Offer);
        return Unit.Value;
    }
}
