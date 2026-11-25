using FluentValidation;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Devices.Commands;

public sealed class NotifyDeviceStatusChangedCommandValidator : AbstractValidator<NotifyDeviceStatusChangedCommand>
{
    public NotifyDeviceStatusChangedCommandValidator(IDeviceAccessProvider devices)
    {
        RuleFor(x => x.AccessToken).NotEmpty().WithMessage(ValidationErrorMessages.TokenRequired);
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceRequired);
        RuleFor(x => x.DeviceId).MustAsync(async (command, id, cancellationToken) =>
                (await devices.GetAccess(id, command.AccessToken, cancellationToken))?.IsOwner == true)
            .When(x => x.DeviceId != Guid.Empty && !string.IsNullOrWhiteSpace(x.AccessToken))
            .WithMessage(ValidationErrorMessages.OwnerRequired);
    }
}
