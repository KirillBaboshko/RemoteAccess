using FluentValidation;
using RemoteAccess.Common.Redis;
using ValidationErrorMessages = RemoteAccess.Signaling.Domain.Exceptions.ValidationErrorMessages;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class InfrastructureSettingsValidator : AbstractValidator<InfrastructureSettings>
{
    public InfrastructureSettingsValidator()
    {
        RuleFor(x => x.RedisSettings).NotNull().WithMessage(ValidationErrorMessages.InfrastructureSettingsRequired)
            .SetValidator(new RedisClientSettingsValidator());
        RuleFor(x => x.SessionSettings).NotNull().WithMessage(ValidationErrorMessages.InfrastructureSettingsRequired)
            .SetValidator(new SessionOptionsValidator());
        RuleFor(x => x.CoreSettings).NotNull().WithMessage(ValidationErrorMessages.InfrastructureSettingsRequired)
            .SetValidator(new CoreApiSettingsValidator());
        RuleFor(x => x.DatabaseName).NotEmpty().WithMessage(ValidationErrorMessages.DatabaseNameRequired);
        RuleFor(x => x.IceSettings).NotNull().WithMessage(ValidationErrorMessages.InfrastructureSettingsRequired)
            .SetValidator(new IceSettingsValidator());
    }
}
