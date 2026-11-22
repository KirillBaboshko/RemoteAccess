using MediatR;
using RemoteAccess.Signaling.Application.Services;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class DisconnectClientHandler : IRequestHandler<DisconnectClientCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;

    public DisconnectClientHandler(ISignalingState state, ISignalingNotifier notifier)
    {
        _state = state;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(DisconnectClientCommand request, CancellationToken cancellationToken)
    {
        var sessions = await _state.DisconnectAsync(request.ConnectionId, cancellationToken);
        await Task.WhenAll(sessions.Select(session => _notifier.SessionUpdated(session)));
        return Unit.Value;
    }
}
