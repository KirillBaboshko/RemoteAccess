using MediatR;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class GetUserDevicesHandler : IRequestHandler<UserDevicesQuery, List<DeviceModel>>
{
    private readonly IDeviceDataProvider _devicesDataProvider;

    public GetUserDevicesHandler(IDeviceDataProvider devicesDataProvider)
    {
        _devicesDataProvider = devicesDataProvider;
    }

    public Task<List<DeviceModel>> Handle(UserDevicesQuery request, CancellationToken cancellationToken)
    {
        return _devicesDataProvider.GetDevicesByOwnerId(request.UserId, cancellationToken);
    }
}
