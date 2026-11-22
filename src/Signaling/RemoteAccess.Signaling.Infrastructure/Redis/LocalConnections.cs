using System.Collections.Concurrent;

namespace RemoteAccess.Signaling.Infrastructure.Redis;
public sealed class LocalConnections
{
    public string NodeId { get; } = Guid.NewGuid().ToString("N");
    private readonly ConcurrentDictionary<string, byte> _connections = new();

    public void Add(string connectionId) => _connections.TryAdd(connectionId, 0);
    public void Remove(string connectionId) => _connections.TryRemove(connectionId, out _);
    public string[] Snapshot() => _connections.Keys.ToArray();
}
