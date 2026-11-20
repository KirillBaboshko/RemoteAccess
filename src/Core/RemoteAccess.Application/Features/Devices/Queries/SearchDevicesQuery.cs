using MediatR;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public record SearchDevicesQuery(string Search) : IRequest<List<DeviceModel>>;
