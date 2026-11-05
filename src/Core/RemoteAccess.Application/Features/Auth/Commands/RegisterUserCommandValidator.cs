using FluentValidation;
using RemoteAccess.Application.Features.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess.Application.Features.Users.Commands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.DataInfo)
                .NotNull()
                .SetValidator(AuthDataInfoValidator.Default);
        }
    }
}
