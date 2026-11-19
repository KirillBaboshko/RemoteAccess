using MediatR;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Application.Features.Devices.Commands;

public record SetDeviceOnlineCommand(DeviceOnlineRequest Device, Guid AuthenticatedUserId) : IRequest<Unit>;
