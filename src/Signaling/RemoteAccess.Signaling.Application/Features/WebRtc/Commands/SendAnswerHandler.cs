using MediatR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Models.WebRtc;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendAnswerHandler : IRequestHandler<SendAnswerCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;

    public SendAnswerHandler(ISignalingState state, ISignalingNotifier notifier)
    {
        _state = state;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(SendAnswerCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.PrepareWebRtcMessageAsync(request.Answer.SessionId,
            request.Caller.UserId, request.Caller.ConnectionId, WebRtcMessageType.Answer, cancellationToken);
        var recipient = session.Source.ConnectionId;
        await _notifier.AnswerReceived(recipient, request.Answer);
        await _notifier.SessionUpdated(session);
        return Unit.Value;
    }
}
