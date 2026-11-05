using FluentValidation;
using RemoteAccess.Application.Features.Devices.Models;

namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class CreateDeviceCommandValidator:AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(x => x.DeviceInfo)
                .NotNull()
                .SetValidator(DeviceInfoValidator.Default);
        }
    }
}
