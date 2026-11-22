using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class ConnectClientCommandValidator : AbstractValidator<ConnectClientCommand>
{
    public ConnectClientCommandValidator() =>
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
}
