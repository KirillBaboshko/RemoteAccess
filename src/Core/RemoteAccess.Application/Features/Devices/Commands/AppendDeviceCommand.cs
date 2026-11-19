using MediatR;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class AppendDeviceCommand : IRequest<DeviceRegistrationResult>
    {
        public DeviceInfo DeviceInfo { get; init; }
        public Guid AuthenticatedUserId { get; }
        public AppendDeviceCommand(DeviceInfo deviceInfo, Guid authenticatedUserId)
        {
            DeviceInfo = deviceInfo;
            AuthenticatedUserId = authenticatedUserId;
        }
    }

}
