using MediatR;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class ConnectClientHandler : IRequestHandler<ConnectClientCommand, Unit>
{
    private readonly ISignalingState _state;

    public ConnectClientHandler(ISignalingState state) => _state = state;

    public async Task<Unit> Handle(ConnectClientCommand request, CancellationToken cancellationToken)
    {
        await _state.ConnectAsync(request.Caller.UserId, request.Caller.ConnectionId, cancellationToken);
        return Unit.Value;
    }
}
