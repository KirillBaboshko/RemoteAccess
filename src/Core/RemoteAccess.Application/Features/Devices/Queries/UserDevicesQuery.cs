using MediatR;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public record UserDevicesQuery(Guid UserId) : IRequest<List<DeviceModel>>;
