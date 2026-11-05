using MediatR;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Common.Data;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Domain.Repositories;

namespace RemoteAccess.Application.Features.Devices.Commands;

public class CreateDeviceHandler : IRequestHandler<CreateDeviceCommand, Guid>
{
    private readonly IDevicesRepository _devicesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceHandler(IDevicesRepository devicesRepository, IUnitOfWork unitOfWork)
    {
        _devicesRepository = devicesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        // Создаём strongly-typed ID для владельца
        var ownerId = new UserId(request.DeviceInfo.OwnerId);
        var device = new Device(ownerId, request.DeviceInfo.Name, request.DeviceInfo.PlatformType);
        await _devicesRepository.CreateDevice(device, cancellationToken);
        
        // Сохранение изменений через UnitOfWork
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return device.Id.Value;
    }
}
