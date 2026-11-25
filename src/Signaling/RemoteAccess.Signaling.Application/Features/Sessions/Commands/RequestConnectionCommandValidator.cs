using FluentValidation;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RequestConnectionCommandValidator : AbstractValidator<RequestConnectionCommand>
{
    public RequestConnectionCommandValidator(IDeviceAccessProvider devices)
    {
        RuleFor(x => x.Caller).NotNull().WithMessage(ValidationErrorMessages.CallerRequired)
            .SetValidator(SignalingCallerValidator.Default);
        RuleFor(x => x.Request).NotNull().WithMessage(ValidationErrorMessages.ConnectionRequestRequired)
            .SetValidator(ConnectionRequestValidator.Default);
        RuleFor(x => x.Request.SourceDeviceId)
            .MustAsync(async (command, id, cancellationToken) =>
                (await devices.GetAccess(id, command.Caller.AccessToken, cancellationToken))?.IsOwner == true)
            .When(x => x.Request is not null && x.Caller is not null && x.Request.SourceDeviceId != Guid.Empty)
            .WithMessage(ValidationErrorMessages.OwnerRequired);
        RuleFor(x => x.Request.TargetDeviceId)
            .MustAsync(async (command, id, cancellationToken) =>
                await devices.GetAccess(id, command.Caller.AccessToken, cancellationToken) is not null)
            .When(x => x.Request is not null && x.Caller is not null && x.Request.TargetDeviceId != Guid.Empty)
            .WithMessage(ValidationErrorMessages.DeviceNotFound);
    }
}
