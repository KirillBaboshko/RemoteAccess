namespace RemoteAccess.Common.Redis.Abstractions;

public sealed class RedisClientException : Exception
{
    public RedisClientException(string message, Exception innerException) : base(message, innerException) { }
}
