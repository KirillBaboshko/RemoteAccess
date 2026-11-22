using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RemoteAccess.Common.Redis.Abstractions;
using StackExchange.Redis;

namespace RemoteAccess.Common.Redis;

public sealed class RedisClient : IRedisClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDatabase _database;
    private readonly RedisClientSettings _settings;
    private readonly ILogger<RedisClient> _logger;

    public RedisClient(IConnectionMultiplexer connection, IOptions<RedisClientSettings> settings, ILogger<RedisClient> logger)
    {
        _database = connection.GetDatabase();
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<T> ExecuteScriptAsync<T>(string script, IReadOnlyList<string> keys,
        IReadOnlyList<string> arguments, object data, CancellationToken cancellationToken)
    {
        var redisKeys = keys.Select(key => (RedisKey)(_settings.InstanceName + key)).ToArray();
        var redisArguments = arguments.Select(argument => (RedisValue)argument)
            .Append(JsonSerializer.Serialize(data, JsonOptions)).ToArray();
        var result = await ExecuteAsync(() => _database.ScriptEvaluateAsync(script, redisKeys, redisArguments), cancellationToken);
        return Deserialize<T>(result.ToString());
    }

    public async Task<IReadOnlyList<RedisStreamEntry<T>>> ReadStreamAsync<T>(string stream, string field,
        string position, int count, CancellationToken cancellationToken)
    {
        var entries = await ExecuteAsync(() => _database.StreamReadAsync(_settings.InstanceName + stream, position, count), cancellationToken);
        return entries.Select(entry => new RedisStreamEntry<T>(entry.Id.ToString(),
            Deserialize<T>(entry.Values.Single(value => value.Name == field).Value.ToString()))).ToArray();
    }

    private async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        try
        {
            return await operation().WaitAsync(cancellationToken);
        }
        catch (RedisException error)
        {
            _logger.LogError(error, "Ошибка выполнения операции Redis");
            throw new RedisClientException(ValidationErrorMessages.OperationFailed, error);
        }
    }

    private static T Deserialize<T>(string value)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(value, JsonOptions) ?? throw new JsonException();
        }
        catch (JsonException error)
        {
            throw new RedisClientException(ValidationErrorMessages.InvalidResponse, error);
        }
    }
}
