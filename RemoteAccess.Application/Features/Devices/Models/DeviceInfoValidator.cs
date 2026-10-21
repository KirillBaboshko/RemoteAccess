using FluentValidation;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Application.Features.Devices.Models
{
    public class DeviceInfoValidator : AbstractValidator<DeviceInfo>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public static readonly DeviceInfoValidator Default = new();
        /// <summary>
        /// Конструктор
        /// </summary>
        public DeviceInfoValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty().WithMessage(ValidationErrorMessages.DeviceOwnerIdRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationErrorMessages.DeviceNameRequired)
                .MaximumLength(64).WithMessage(ValidationErrorMessages.ExceededMaxLength + "64 для логина");
            RuleFor(x => x.PlatformType).IsInEnum().WithMessage(ValidationErrorMessages.DeviceInvalidPlatformType);
        }
    }
}
