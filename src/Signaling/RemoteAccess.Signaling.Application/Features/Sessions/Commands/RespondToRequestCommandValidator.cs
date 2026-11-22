using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RespondToRequestCommandValidator : AbstractValidator<RespondToRequestCommand>
{
    public RespondToRequestCommandValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
    }
}
