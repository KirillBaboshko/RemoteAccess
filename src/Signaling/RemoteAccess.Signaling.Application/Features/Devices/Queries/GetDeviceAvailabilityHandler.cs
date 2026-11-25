using MediatR;
using RemoteAccess.Signaling.Domain.Models.Devices;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Devices.Queries;

public sealed class GetDeviceAvailabilityHandler : IRequestHandler<DeviceAvailabilityQuery, IReadOnlyList<DeviceAvailability>>
{
    private readonly ISignalingState _state;

    public GetDeviceAvailabilityHandler(ISignalingState state) => _state = state;

    public Task<IReadOnlyList<DeviceAvailability>> Handle(DeviceAvailabilityQuery request, CancellationToken cancellationToken) =>
        _state.GetDeviceAvailabilityAsync(request.DeviceIds, cancellationToken);
}
