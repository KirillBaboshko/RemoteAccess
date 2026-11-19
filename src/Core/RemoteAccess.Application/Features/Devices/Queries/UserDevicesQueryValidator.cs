using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Queries;

public sealed class UserDevicesQueryValidator : AbstractValidator<UserDevicesQuery>
{
    public UserDevicesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.UserIdRequired);
    }
}
