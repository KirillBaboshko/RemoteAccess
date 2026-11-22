using MediatR;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class RegisterHostHandler : IRequestHandler<RegisterHostCommand, Unit>
{
    private readonly ISignalingState _state;

    public RegisterHostHandler(ISignalingState state) => _state = state;

    public async Task<Unit> Handle(RegisterHostCommand request, CancellationToken cancellationToken)
    {
        await _state.RegisterHostAsync(new SessionParticipant(request.Caller.UserId, request.DeviceId, request.Caller.ConnectionId), cancellationToken);
        return Unit.Value;
    }
}
