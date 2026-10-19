using FluentValidation;
using RemoteAccess.Application.Features.Users.Models;

namespace RemoteAccess.Application.Features.Users.Commands
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.DataInfo)
                .NotNull()
                .SetValidator(AuthDataInfoValidator.Default);
        }
    }
}
