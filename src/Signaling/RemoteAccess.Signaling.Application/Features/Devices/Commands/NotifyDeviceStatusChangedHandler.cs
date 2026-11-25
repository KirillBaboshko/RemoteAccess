using MediatR;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Devices.Commands;

public sealed class NotifyDeviceStatusChangedHandler : IRequestHandler<NotifyDeviceStatusChangedCommand, Unit>
{
    private readonly ISignalingState _state;
    private readonly IDeviceAccessProvider _devices;

    public NotifyDeviceStatusChangedHandler(ISignalingState state, IDeviceAccessProvider devices)
    {
        _state = state;
        _devices = devices;
    }

    public async Task<Unit> Handle(NotifyDeviceStatusChangedCommand request, CancellationToken cancellationToken)
    {
        var access = await _devices.GetAccess(request.DeviceId, request.AccessToken, cancellationToken);
        await _state.UpdateDeviceStatusAsync(request.DeviceId, access?.IsOnline == true, cancellationToken);
        return Unit.Value;
    }
}
