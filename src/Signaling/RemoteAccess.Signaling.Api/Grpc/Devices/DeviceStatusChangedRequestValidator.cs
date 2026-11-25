using FluentValidation;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Api.Grpc.Devices;

public sealed class DeviceStatusChangedRequestValidator : AbstractValidator<DeviceStatusChangedRequest>
{
    public DeviceStatusChangedRequestValidator()
    {
        RuleFor(x => x.DeviceId).Must(id => Guid.TryParse(id, out var value) && value != Guid.Empty)
            .WithMessage(ValidationErrorMessages.DeviceRequired);
    }
}
