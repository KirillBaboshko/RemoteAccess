using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Application.Services;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public sealed class RegisterHostCommandValidator : AbstractValidator<RegisterHostCommand>
{
    public RegisterHostCommandValidator(IDeviceAccessProvider devices)
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceRequired)
            .MustAsync(async (command, id, cancellationToken) =>
                (await devices.GetAccess(id, command.Caller.AccessToken, cancellationToken))?.IsOwner == true)
            .WithMessage(ValidationErrorMessages.OwnerRequired)
            .When(x => x.Caller is not null);
    }
}
