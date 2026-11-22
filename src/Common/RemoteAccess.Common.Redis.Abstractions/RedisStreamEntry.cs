namespace RemoteAccess.Common.Redis.Abstractions;

public record RedisStreamEntry<T>(string Id, T Value);
