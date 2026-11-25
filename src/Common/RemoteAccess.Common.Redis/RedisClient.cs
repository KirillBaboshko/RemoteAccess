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
        string position, int count, CancellationToken cancellationToken) where T : class
    {
        var entries = await ExecuteAsync(() => _database.StreamReadAsync(_settings.InstanceName + stream, position, count), cancellationToken);
        return entries.Select(entry =>
        {
            try
            {
                return new RedisStreamEntry<T>(entry.Id.ToString(),
                    Deserialize<T>(entry.Values.Single(value => value.Name == field).Value.ToString()));
            }
            catch (Exception error) when (error is RedisClientException or InvalidOperationException)
            { 
                _logger.LogError(error, "Пропущена некорректная запись Redis {Stream}/{EntryId}", stream, entry.Id);
                return new RedisStreamEntry<T>(entry.Id.ToString(), null);
            }
        }).ToArray();
    }

    public async Task<string> GetStreamPositionAsync(string stream, CancellationToken cancellationToken)
    {
        var entries = await ExecuteAsync(() => _database.StreamRangeAsync(_settings.InstanceName + stream,
            count: 1, messageOrder: Order.Descending), cancellationToken);
        return entries.Length == 0 ? "0-0" : entries[0].Id.ToString();
    }

    private async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        try
        {
            return await operation().WaitAsync(cancellationToken);
        }
        catch (Exception error) when (error is RedisException or RedisTimeoutException)
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
