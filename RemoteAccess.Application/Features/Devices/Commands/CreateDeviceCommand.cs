using MediatR;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class CreateDeviceCommand: IRequest<Guid>
    {
        public DeviceInfo DeviceInfo { get; init; }
        public CreateDeviceCommand(DeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }
    }
}
