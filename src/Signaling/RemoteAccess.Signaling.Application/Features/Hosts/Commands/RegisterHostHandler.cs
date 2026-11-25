using MediatR;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class RegisterHostHandler : IRequestHandler<RegisterHostCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly IDeviceAccessProvider _devices;

    public RegisterHostHandler(ISignalingState state, IDeviceAccessProvider devices)
    {
        _state = state;
        _devices = devices;
    }

    public async Task<Unit> Handle(RegisterHostCommand request, CancellationToken cancellationToken)
    {
        var access = await _devices.GetAccess(request.DeviceId, request.Caller.AccessToken, cancellationToken);
        await _state.UpdateDeviceStatusAsync(request.DeviceId, access?.IsOnline == true, cancellationToken);
        await _state.RegisterHostAsync(new SessionParticipant(request.Caller.UserId, request.DeviceId, request.Caller.ConnectionId), cancellationToken);
        return Unit.Value;
    }
}
