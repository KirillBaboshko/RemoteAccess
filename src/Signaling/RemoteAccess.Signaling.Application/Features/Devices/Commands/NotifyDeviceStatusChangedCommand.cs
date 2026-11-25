using MediatR;

namespace RemoteAccess.Signaling.Application.Features.Devices.Commands;

public record NotifyDeviceStatusChangedCommand(Guid DeviceId, string AccessToken) : IRequest<Unit>;
