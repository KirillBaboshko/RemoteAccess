using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Models;

public sealed class DeviceStatusRequestValidator : AbstractValidator<DeviceStatusRequest>
{
    public static readonly DeviceStatusRequestValidator Default = new();

    public DeviceStatusRequestValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceOwnerIdRequired);
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceIdRequired);
    }
}
