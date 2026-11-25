using FluentValidation;
using RemoteAccess.Contracts.Signaling.Devices;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Api.Grpc.Devices;

public sealed class GetDeviceAvailabilityRequestValidator : AbstractValidator<GetDeviceAvailabilityRequest>
{
    public GetDeviceAvailabilityRequestValidator()
    {
        RuleFor(x => x.DeviceIds).NotEmpty().WithMessage(ValidationErrorMessages.DeviceRequired)
            .Must(ids => ids.Count <= DeviceAvailabilityLimits.BatchSize)
            .WithMessage(ValidationErrorMessages.AvailabilityBatchTooLarge);
        RuleForEach(x => x.DeviceIds).Must(id => Guid.TryParse(id, out var value) && value != Guid.Empty)
            .WithMessage(ValidationErrorMessages.DeviceRequired);
    }
}
