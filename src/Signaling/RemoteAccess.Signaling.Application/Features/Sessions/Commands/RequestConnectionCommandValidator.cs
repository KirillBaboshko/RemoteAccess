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
            .CustomAsync(async (id, context, cancellationToken) =>
            {
                var access = await devices.GetAccess(id, context.InstanceToValidate.Caller.AccessToken, cancellationToken);
                if (access?.IsOwner != true) context.AddFailure(ValidationErrorMessages.OwnerRequired);
                else if (!access.IsOnline) context.AddFailure(ValidationErrorMessages.DeviceOffline);
            })
            .When(x => x.Request is not null && x.Caller is not null && x.Request.SourceDeviceId != Guid.Empty);
        RuleFor(x => x.Request.TargetDeviceId)
            .CustomAsync(async (id, context, cancellationToken) =>
            {
                var access = await devices.GetAccess(id, context.InstanceToValidate.Caller.AccessToken, cancellationToken);
                if (access is null) context.AddFailure(ValidationErrorMessages.DeviceNotFound);
                else if (!access.IsOnline) context.AddFailure(ValidationErrorMessages.DeviceOffline);
            })
            .When(x => x.Request is not null && x.Caller is not null && x.Request.TargetDeviceId != Guid.Empty);
    }
}
