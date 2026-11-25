using MediatR;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class DisconnectClientHandler : IRequestHandler<DisconnectClientCommand, Unit>
{
    private readonly ISignalingState _state;

    public DisconnectClientHandler(ISignalingState state) => _state = state;

    public async Task<Unit> Handle(DisconnectClientCommand request, CancellationToken cancellationToken)
    {
        await _state.DisconnectAsync(request.ConnectionId, cancellationToken);
        return Unit.Value;
    }
}
