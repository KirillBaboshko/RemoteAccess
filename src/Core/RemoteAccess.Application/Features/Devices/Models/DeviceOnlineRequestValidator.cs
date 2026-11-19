using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Models;

public sealed class DeviceOnlineRequestValidator : AbstractValidator<DeviceOnlineRequest>
{
    public static readonly DeviceOnlineRequestValidator Default = new();

    public DeviceOnlineRequestValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceOwnerIdRequired);
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceIdRequired);
    }
}
