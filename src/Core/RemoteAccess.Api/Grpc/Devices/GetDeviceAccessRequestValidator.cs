using FluentValidation;
using RemoteAccess.Contracts.Core.Devices;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Api.Grpc.Devices;

public sealed class GetDeviceAccessRequestValidator : AbstractValidator<GetDeviceAccessRequest>
{
    public GetDeviceAccessRequestValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceIdRequired)
            .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
            .WithMessage(ValidationErrorMessages.DeviceIdInvalid);
    }
}
