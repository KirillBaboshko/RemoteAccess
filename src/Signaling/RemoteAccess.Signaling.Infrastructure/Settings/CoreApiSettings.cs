namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class CoreApiSettings
{
    public Uri BaseUrl { get; init; } = new("about:blank");
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);
}
