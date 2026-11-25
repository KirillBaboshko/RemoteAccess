using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Validators;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendAnswerCommandValidator : AbstractValidator<SendAnswerCommand>
{
    public SendAnswerCommandValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.Answer).NotNull().WithMessage(ValidationErrorMessages.SessionDescriptionRequired)
            .SetValidator(SessionDescriptionMessageValidator.Default);
    }
}
