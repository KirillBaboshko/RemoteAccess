using RemoteAccess.Domain.Models.Ecxeptions;
using FluentValidation;

namespace RemoteAccess.Application.Features.Users.Queries
{
    public class UserProfileQueryValidator:AbstractValidator<UserProfileQuery>
    {
        public UserProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(ValidationErrorMessages.UserIdRequired)
                .Must(id => id != Guid.Empty).WithMessage(ValidationErrorMessages.UserIdNotNull);
        }
    }
}
