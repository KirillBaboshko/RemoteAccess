using MediatR;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Common.Data;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Domain.Repositories;

namespace RemoteAccess.Application.Features.Devices.Commands;

public class AppendDeviceHandler : IRequestHandler<AppendDeviceCommand, DeviceRegistrationResult>
{
    private readonly IDevicesRepository _devicesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppendDeviceHandler(IDevicesRepository devicesRepository, IUnitOfWork unitOfWork)
    {
        _devicesRepository = devicesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeviceRegistrationResult> Handle(AppendDeviceCommand request, CancellationToken cancellationToken)
    {
        var ownerId = new UserId(request.DeviceInfo.OwnerId);
        var device = new Device(ownerId, request.DeviceInfo.Name, request.DeviceInfo.PlatformType);
        await _devicesRepository.CreateDevice(device, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new DeviceRegistrationResult(device.Id.Value, ownerId.Value);
    }
}
