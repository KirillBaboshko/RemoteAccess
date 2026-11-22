using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class DeviceAccessQueryValidator : AbstractValidator<DeviceAccessQuery>
{
    public DeviceAccessQueryValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceIdRequired);
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.UserIdRequired);
    }
}
