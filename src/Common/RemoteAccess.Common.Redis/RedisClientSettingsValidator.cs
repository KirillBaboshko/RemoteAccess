using FluentValidation;

namespace RemoteAccess.Common.Redis;

public sealed class RedisClientSettingsValidator : AbstractValidator<RedisClientSettings>
{
    public RedisClientSettingsValidator()
    {
        RuleFor(x => x.Configuration).NotEmpty().WithMessage(ValidationErrorMessages.ConfigurationRequired);
        RuleFor(x => x.InstanceName).NotEmpty().WithMessage(ValidationErrorMessages.InstanceNameRequired);
    }
}
