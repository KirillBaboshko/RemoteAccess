using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Repositories;

namespace RemoteAccess.Application.Features.Devices.Commands;

public sealed class SetDeviceOnlineHandler : IRequestHandler<SetDeviceOnlineCommand, Unit>
{
    private readonly IDevicesRepository _devicesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetDeviceOnlineHandler(IDevicesRepository devicesRepository, IUnitOfWork unitOfWork)
    {
        _devicesRepository = devicesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(SetDeviceOnlineCommand request, CancellationToken cancellationToken)
    {
        var device = await _devicesRepository.GetDeviceById(new DeviceId(request.Device.DeviceId), cancellationToken)
            ?? throw new InvalidOperationException("Device was removed during status update.");
        device.DeviceOn();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
