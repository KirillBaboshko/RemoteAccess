using MediatR;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class AppendDeviceCommand : IRequest<Guid>
    {
        public DeviceInfo DeviceInfo { get; init; }
        public AppendDeviceCommand(DeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }
    }
}