using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Users.Models
{
    public class AuthDataInfoValidator : AbstractValidator<AuthDataInfo>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public static readonly AuthDataInfoValidator Default = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public AuthDataInfoValidator()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage(ValidationErrorMessages.LoginRequired)
                .MaximumLength(64).WithMessage(ValidationErrorMessages.ExceededMaxLength + "64 для логина");
            RuleFor(x => x.Password).NotEmpty().WithMessage(ValidationErrorMessages.PasswordRequired)
                .MaximumLength(64).WithMessage(ValidationErrorMessages.ExceededMaxLength + "64 для пароля");
        }
    }
}
