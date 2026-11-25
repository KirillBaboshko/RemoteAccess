using System.Text;
using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Validators;

public sealed class SessionDescriptionMessageValidator : AbstractValidator<SessionDescriptionMessage>
{
    public static readonly SessionDescriptionMessageValidator Default = new();

    public SessionDescriptionMessageValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
        RuleFor(x => x.Sdp).NotEmpty().WithMessage(ValidationErrorMessages.SdpRequired)
            .Must(sdp => sdp is not null && Encoding.UTF8.GetByteCount(sdp) <= WebRtcMessageLimits.SdpBytes)
            .WithMessage(ValidationErrorMessages.SdpTooLong);
    }
}
