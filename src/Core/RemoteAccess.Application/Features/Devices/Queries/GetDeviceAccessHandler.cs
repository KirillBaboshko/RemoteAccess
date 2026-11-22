using MediatR;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class GetDeviceAccessHandler : IRequestHandler<DeviceAccessQuery, DeviceAccessModel?>
{
    private readonly IDeviceDataProvider _devices;

    public GetDeviceAccessHandler(IDeviceDataProvider devices) => _devices = devices;

    public Task<DeviceAccessModel?> Handle(DeviceAccessQuery request, CancellationToken cancellationToken) =>
        _devices.GetDeviceAccess(request.DeviceId, request.UserId, cancellationToken);
}
