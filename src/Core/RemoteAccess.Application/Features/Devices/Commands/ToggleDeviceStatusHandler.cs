using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Ecxeptions;
using RemoteAccess.Domain.Repositories;

namespace RemoteAccess.Application.Features.Devices.Commands;

public sealed class ToggleDeviceStatusHandler : IRequestHandler<ToggleDeviceStatusCommand, DeviceStatus>
{
    private readonly IDevicesRepository _devicesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleDeviceStatusHandler(IDevicesRepository devicesRepository, IUnitOfWork unitOfWork)
    {
        _devicesRepository = devicesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeviceStatus> Handle(ToggleDeviceStatusCommand request, CancellationToken cancellationToken)
    {
        var device = await _devicesRepository.GetDeviceById(new DeviceId(request.Device.DeviceId), cancellationToken)
            ?? throw new InvalidOperationException(ValidationErrorMessages.DeviceRemovedDuringStatusUpdate);
        device.ToggleStatus();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return device.Status;
    }
}
