namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class IceSettings
{
    public string[] StunUrls { get; init; } = [];
    public string[] TurnUrls { get; init; } = [];
    public string SharedSecret { get; init; } = string.Empty;
    public TimeSpan CredentialLifetime { get; init; } = TimeSpan.FromHours(1);
}
