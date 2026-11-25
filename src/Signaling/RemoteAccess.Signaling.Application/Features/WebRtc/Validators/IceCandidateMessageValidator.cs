using System.Text;
using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Validators;

public sealed class IceCandidateMessageValidator : AbstractValidator<IceCandidateMessage>
{
    public static readonly IceCandidateMessageValidator Default = new();

    public IceCandidateMessageValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
        // null и пустая строка обозначают завершение передачи ICE-кандидатов.
        RuleFor(x => x.Candidate)
            .Must(candidate => candidate is null || candidate.Length == 0 || !string.IsNullOrWhiteSpace(candidate))
            .WithMessage(ValidationErrorMessages.IceCandidateInvalid)
            .Must(candidate => candidate is null || Encoding.UTF8.GetByteCount(candidate) <= WebRtcMessageLimits.IceCandidateBytes)
            .WithMessage(ValidationErrorMessages.IceCandidateTooLong);
        RuleFor(x => x.SdpMid).MaximumLength(WebRtcMessageLimits.IceFieldLength)
            .WithMessage(ValidationErrorMessages.IceFieldTooLong);
        RuleFor(x => x.UsernameFragment).MaximumLength(WebRtcMessageLimits.IceFieldLength)
            .WithMessage(ValidationErrorMessages.IceFieldTooLong);
        RuleFor(x => x.SdpMLineIndex).InclusiveBetween(0, ushort.MaxValue)
            .WithMessage(ValidationErrorMessages.IceCandidateInvalid);
        RuleFor(x => x).Must(message => string.IsNullOrEmpty(message.Candidate)
                || message.SdpMid is not null || message.SdpMLineIndex.HasValue)
            .WithMessage(ValidationErrorMessages.IceMediaRequired);
    }
}
