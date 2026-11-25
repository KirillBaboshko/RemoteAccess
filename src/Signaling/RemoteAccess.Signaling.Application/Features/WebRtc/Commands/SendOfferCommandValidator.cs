using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Validators;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class SendOfferCommandValidator : AbstractValidator<SendOfferCommand>
{
    public SendOfferCommandValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.Offer).NotNull().WithMessage(ValidationErrorMessages.SessionDescriptionRequired)
            .SetValidator(SessionDescriptionMessageValidator.Default);
    }
}
