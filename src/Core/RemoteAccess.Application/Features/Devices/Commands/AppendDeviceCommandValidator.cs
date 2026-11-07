using FluentValidation;
using RemoteAccess.Application.Features.Devices.Models;
namespace RemoteAccess.Application.Features.Devices.Commands
{
    public class AppendDeviceCommandValidator : AbstractValidator<AppendDeviceCommand>
    {
        public AppendDeviceCommandValidator()
        {
            RuleFor(x => x.DeviceInfo)
                .NotNull()
                .SetValidator(DeviceInfoValidator.Default);
        }
    }
}
