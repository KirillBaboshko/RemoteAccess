namespace RemoteAccess.Infrastructure.Settings;

public sealed class SignalingApiSettings
{
    public Uri BaseUrl { get; set; } = new("http://localhost:5101/");
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
}
