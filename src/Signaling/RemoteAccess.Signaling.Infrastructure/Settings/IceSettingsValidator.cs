using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class IceSettingsValidator : AbstractValidator<IceSettings>
{
    private static readonly Regex IceUrl = new(
        @"\A(?<scheme>stuns?|turns?):(?<host>\[[^\]]+\]|[^:/?#@\s]+)(?::(?<port>[0-9]+))?(?<transport>\?transport=(?:udp|tcp))?\z",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public IceSettingsValidator()
    {
        RuleFor(x => x.StunUrls).NotNull().WithMessage(ValidationErrorMessages.IceUrlsRequired);
        RuleFor(x => x.TurnUrls).NotNull().WithMessage(ValidationErrorMessages.IceUrlsRequired);
        RuleForEach(x => x.StunUrls).Must(url => IsValidUrl(url, "stun"))
            .WithMessage(ValidationErrorMessages.StunUrlInvalid);
        RuleForEach(x => x.TurnUrls).Must(url => IsValidUrl(url, "turn"))
            .WithMessage(ValidationErrorMessages.TurnUrlInvalid);
        RuleFor(x => x.SharedSecret).NotEmpty().WithMessage(ValidationErrorMessages.TurnSecretRequired)
            .When(x => x.TurnUrls is { Length: > 0 });
        RuleFor(x => x.CredentialLifetime).InclusiveBetween(TimeSpan.FromSeconds(1), TimeSpan.FromDays(1))
            .WithMessage(ValidationErrorMessages.TurnLifetimeInvalid);
    }

    private static bool IsValidUrl(string? value, string scheme)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        var match = IceUrl.Match(value);
        if (!match.Success || !match.Groups["scheme"].Value.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
            return false;
        if (scheme == "stun" && match.Groups["transport"].Success) return false;
        var host = match.Groups["host"].Value;
        var hostType = Uri.CheckHostName(host.Trim('[', ']'));
        if (host.StartsWith('[') ? hostType != UriHostNameType.IPv6
            : hostType is not (UriHostNameType.Dns or UriHostNameType.IPv4)) return false;
        return !match.Groups["port"].Success || int.TryParse(match.Groups["port"].Value,
            NumberStyles.None, CultureInfo.InvariantCulture, out var port) && port is > 0 and <= 65535;
    }
}
