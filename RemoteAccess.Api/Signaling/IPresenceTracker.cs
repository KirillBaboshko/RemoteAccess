namespace RemoteAccess.Api.Signaling;

public interface IPresenceTracker
{
    void Add(Guid userId, string email, string connectionId);
    void Remove(string connectionId);
    IReadOnlyList<string> GetConnectionIds(Guid userId);
    IReadOnlyList<OnlineUserDto> GetOnlineUsers();
    string? GetEmail(Guid userId);
}

public sealed class PresenceTracker : IPresenceTracker
{
    private readonly object _sync = new();
    private readonly Dictionary<string, Guid> _connectionToUser = new();
    private readonly Dictionary<Guid, HashSet<string>> _userConnections = new();
    private readonly Dictionary<Guid, string> _emails = new();

    public void Add(Guid userId, string email, string connectionId)
    {
        lock (_sync)
        {
            _connectionToUser[connectionId] = userId;
            if (!_userConnections.TryGetValue(userId, out var connections))
            {
                connections = [];
                _userConnections[userId] = connections;
            }

            connections.Add(connectionId);
            _emails[userId] = email;
        }
    }

    public void Remove(string connectionId)
    {
        lock (_sync)
        {
            if (!_connectionToUser.Remove(connectionId, out var userId))
            {
                return;
            }

            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _userConnections.Remove(userId);
                    _emails.Remove(userId);
                }
            }
        }
    }

    public IReadOnlyList<string> GetConnectionIds(Guid userId)
    {
        lock (_sync)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections.ToArray()
                : [];
        }
    }

    public IReadOnlyList<OnlineUserDto> GetOnlineUsers()
    {
        lock (_sync)
        {
            return _userConnections.Keys
                .Select(id => new OnlineUserDto(id, _emails.GetValueOrDefault(id, string.Empty)))
                .OrderBy(u => u.Email)
                .ToArray();
        }
    }

    public string? GetEmail(Guid userId)
    {
        lock (_sync)
        {
            return _emails.GetValueOrDefault(userId);
        }
    }
}
