using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Validators;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendIceCandidateCommandValidator : AbstractValidator<SendIceCandidateCommand>
{
    public SendIceCandidateCommandValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.Candidate).NotNull().WithMessage(ValidationErrorMessages.IceMessageRequired)
            .SetValidator(IceCandidateMessageValidator.Default);
    }
}
