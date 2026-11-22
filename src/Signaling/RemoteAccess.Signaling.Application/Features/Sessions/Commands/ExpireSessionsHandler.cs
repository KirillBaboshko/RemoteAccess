using MediatR;
using RemoteAccess.Signaling.Application.Services;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class ExpireSessionsHandler : IRequestHandler<ExpireSessionsCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;

    public ExpireSessionsHandler(ISignalingState state, ISignalingNotifier notifier)
    {
        _state = state;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(ExpireSessionsCommand request, CancellationToken cancellationToken)
    {
        var sessions = await _state.ExpireSessionsAsync(cancellationToken);
        await Task.WhenAll(sessions.Select(session => _notifier.SessionUpdated(session)));
        return Unit.Value;
    }
}
