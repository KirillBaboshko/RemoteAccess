using System.Collections.Concurrent;
using RemoteAccess.Signaling.Infrastructure.Redis.Models;

namespace RemoteAccess.Signaling.Infrastructure.Redis;
public sealed class LocalConnections
{
    public string NodeId { get; } = Guid.NewGuid().ToString("N");
    private readonly ConcurrentDictionary<string, LocalConnection> _connections = new();

    public void Add(Guid userId, string connectionId) => _connections.TryAdd(connectionId, new(userId, connectionId));
    public void RegisterHost(string connectionId, Guid deviceId)
    {
        if (_connections.TryGetValue(connectionId, out var connection))
            _connections.TryUpdate(connectionId, connection with { HostDeviceId = deviceId.ToString() }, connection);
    }
    public void Remove(string connectionId) => _connections.TryRemove(connectionId, out _);
    public bool Contains(string connectionId) => _connections.ContainsKey(connectionId);
    public string[] Snapshot() => _connections.Keys.ToArray();
    public LocalConnection[] Registrations() => _connections.Values.ToArray();
}
