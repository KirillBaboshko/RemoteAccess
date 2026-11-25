using FluentValidation;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class ReportConnectionStateCommandValidator : AbstractValidator<ReportConnectionStateCommand>
{
    public ReportConnectionStateCommandValidator()
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.Message).NotNull().WithMessage(ValidationErrorMessages.ConnectionStateRequired)
            .SetValidator(ConnectionStateMessageValidator.Default);
    }
}
