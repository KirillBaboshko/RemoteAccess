using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Infrastructure.Redis;

public static class SignalingRedisScripts
{
    private static readonly string Shared = ReadResource("Shared");

    public static string Connect { get; } = Load("Connect");
    public static string RegisterHost { get; } = Load("RegisterHost");
    public static string CreateSession { get; } = Load("CreateSession");
    public static string RespondToRequest { get; } = Load("RespondToRequest");
    public static string EndSession { get; } = Load("EndSession");
    public static string Disconnect { get; } = Load("Disconnect");
    public static string RenewLeases { get; } = Load("RenewLeases");
    public static string ExpireSessions { get; } = Load("ExpireSessions");

    private static string Load(string name) => Shared + "\n" + ReadResource(name);

    private static string ReadResource(string name)
    {
        using var stream = typeof(SignalingRedisScripts).Assembly.GetManifestResourceStream(
            $"RemoteAccess.Signaling.Infrastructure.Redis.Scripts.{name}.lua")
            ?? throw new InvalidOperationException(string.Format(ValidationErrorMessages.RedisScriptMissing, name));
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
