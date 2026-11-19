using FluentValidation;
using RemoteAccess.Application.Features.Devices.Models;
using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Ecxeptions;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Domain.Repositories;

namespace RemoteAccess.Application.Features.Devices.Commands;

public sealed class ToggleDeviceStatusCommandValidator : AbstractValidator<ToggleDeviceStatusCommand>
{
    public ToggleDeviceStatusCommandValidator(IDevicesRepository devicesRepository)
    {
        RuleFor(x => x.Device)
            .NotNull()
            .SetValidator(DeviceStatusRequestValidator.Default);

        RuleFor(x => x.Device.OwnerId)
            .Equal(x => x.AuthenticatedUserId)
            .When(x => x.Device is not null)
            .WithMessage(ValidationErrorMessages.DeviceOwnerMismatch);

        RuleFor(x => x.Device)
            .MustAsync(async (request, device, cancellationToken) =>
            {
                var existing = await devicesRepository.GetDeviceById(new DeviceId(device.DeviceId), cancellationToken);
                return existing?.OwnerId == new UserId(request.AuthenticatedUserId);
            })
            .When(x => x.Device is not null && x.Device.DeviceId != Guid.Empty)
            .WithMessage(ValidationErrorMessages.DeviceNotOwnedByUser);
    }
}
