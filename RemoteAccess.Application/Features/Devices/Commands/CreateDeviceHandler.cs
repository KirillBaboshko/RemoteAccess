using MediatR;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Domain.Repositories;
using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class CreateDeviceHandler: IRequestHandler<CreateDeviceCommand, Guid>
    {
        private readonly IDevicesRepository _devicesRepository;

        public CreateDeviceHandler(IDevicesRepository devicesRepository)
        {
            _devicesRepository = devicesRepository;
        }
        public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = new Device(request.DeviceInfo.OwnerId, request.DeviceInfo.Name, request.DeviceInfo.PlatformType);
            await _devicesRepository.CreateDevice(device, cancellationToken);
            return device.Id;
        }
    }
}
