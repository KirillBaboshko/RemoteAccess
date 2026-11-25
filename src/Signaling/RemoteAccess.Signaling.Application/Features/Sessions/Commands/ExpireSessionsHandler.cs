using MediatR;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class ExpireSessionsHandler : IRequestHandler<ExpireSessionsCommand, Unit>
{
    private readonly ISignalingState _state;

    public ExpireSessionsHandler(ISignalingState state) => _state = state;

    public async Task<Unit> Handle(ExpireSessionsCommand request, CancellationToken cancellationToken)
    {
        await _state.ExpireSessionsAsync(cancellationToken);
        return Unit.Value;
    }
}
