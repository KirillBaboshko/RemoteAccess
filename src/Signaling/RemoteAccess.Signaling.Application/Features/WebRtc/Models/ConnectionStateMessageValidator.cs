using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Models;

public sealed class ConnectionStateMessageValidator : AbstractValidator<ConnectionStateMessage>
{
    public static readonly ConnectionStateMessageValidator Default = new();

    public ConnectionStateMessageValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage(ValidationErrorMessages.SessionRequired);
        RuleFor(x => x.State).IsInEnum().WithMessage(ValidationErrorMessages.ConnectionStateInvalid);
    }
}
