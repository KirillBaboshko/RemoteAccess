using MediatR;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Application.Features.Devices.Commands;

public record ToggleDeviceStatusCommand(DeviceStatusRequest Device, Guid AuthenticatedUserId) : IRequest<DeviceStatus>;
