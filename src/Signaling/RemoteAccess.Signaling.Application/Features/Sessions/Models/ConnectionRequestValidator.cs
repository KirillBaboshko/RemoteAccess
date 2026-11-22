using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public sealed class ConnectionRequestValidator : AbstractValidator<ConnectionRequest>
{
    public static readonly ConnectionRequestValidator Default = new();

    public ConnectionRequestValidator()
    {
        RuleFor(x => x.SourceDeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceRequired);
        RuleFor(x => x.TargetDeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceRequired)
            .NotEqual(x => x.SourceDeviceId).WithMessage(ValidationErrorMessages.SelfConnection);
    }
}
