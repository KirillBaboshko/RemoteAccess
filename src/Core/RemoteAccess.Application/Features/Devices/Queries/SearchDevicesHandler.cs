using MediatR;
using RemoteAccess.Data.DataProviders.Devices;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class SearchDevicesHandler : IRequestHandler<SearchDevicesQuery, List<DeviceModel>>
{
    private readonly IDeviceDataProvider _devicesDataProvider;

    public SearchDevicesHandler(IDeviceDataProvider devicesDataProvider)
    {
        _devicesDataProvider = devicesDataProvider;
    }

    public Task<List<DeviceModel>> Handle(SearchDevicesQuery request, CancellationToken cancellationToken)
    {
        return _devicesDataProvider.SearchDevices(request.Search.Trim(), cancellationToken);
    }
}
