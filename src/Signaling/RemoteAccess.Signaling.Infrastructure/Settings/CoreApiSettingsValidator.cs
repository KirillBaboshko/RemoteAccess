using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class CoreApiSettingsValidator : AbstractValidator<CoreApiSettings>
{
    public CoreApiSettingsValidator()
    {
        RuleFor(x => x.BaseUrl).NotNull().WithMessage(ValidationErrorMessages.CoreAddressRequired)
            .Must(uri => uri is not null && uri.IsAbsoluteUri && uri.Scheme is "http" or "https")
            .WithMessage(ValidationErrorMessages.CoreAddressRequired);
        RuleFor(x => x.Timeout).GreaterThan(TimeSpan.Zero).WithMessage(ValidationErrorMessages.CoreTimeoutInvalid);
    }
}
