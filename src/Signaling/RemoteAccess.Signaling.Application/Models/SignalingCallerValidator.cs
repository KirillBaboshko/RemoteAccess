using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Models;

public sealed class SignalingCallerValidator : AbstractValidator<SignalingCaller>
{
    public static readonly SignalingCallerValidator Default = new();

    public SignalingCallerValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(ValidationErrorMessages.UserRequired);
        RuleFor(x => x.ConnectionId).NotEmpty().WithMessage(ValidationErrorMessages.ConnectionRequired);
        RuleFor(x => x.AccessToken).NotEmpty().WithMessage(ValidationErrorMessages.TokenRequired);
    }
}
