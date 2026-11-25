namespace RemoteAccess.Signaling.Infrastructure.Redis;

/// <summary>
/// Ключи Signaling без общего префикса экземпляра Redis.
/// </summary>
public static class SignalingRedisKeys
{
    public const string Connections = "connection:";
    public const string Hosts = "host:";
    public const string Sessions = "session:";
    public const string BusyDevices = "busy:";
    public const string ConnectionSessions = "connection-sessions:";
    public const string Deadlines = "deadlines";
    public const string SessionEvents = "session-events";
    public const string AvailabilityEvents = "availability-events";
    public const string HostDeadlines = "host-deadlines";
    public const string ClosedConnections = "closed-connection:";
    public const string SessionEventPositions = "session-event-positions";
    public const string OfflineDevices = "offline-device:";
    public const string Backplane = "backplane";

    public static IReadOnlyList<string> StateKeys { get; } =
        [Connections, Hosts, Sessions, BusyDevices, ConnectionSessions, Deadlines, SessionEvents,
            AvailabilityEvents, HostDeadlines, ClosedConnections, SessionEventPositions, OfflineDevices];
}
