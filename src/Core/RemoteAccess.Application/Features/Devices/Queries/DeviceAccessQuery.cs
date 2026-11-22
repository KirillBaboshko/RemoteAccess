using MediatR;
using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Application.Features.Devices.Queries;

public record DeviceAccessQuery(Guid DeviceId, Guid UserId) : IRequest<DeviceAccessModel?>;
