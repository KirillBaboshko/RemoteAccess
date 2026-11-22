namespace RemoteAccess.Common.Redis.Abstractions;

/// <summary>
/// Типизированные операции Redis без зависимости от конкретной библиотеки клиента.
/// </summary>
public interface IRedisClient
{
    /// <summary>
    /// Выполнить атомарный скрипт. Данные сериализуются в последний аргумент, к ключам добавляется префикс экземпляра.
    /// </summary>
    Task<T> ExecuteScriptAsync<T>(string script, IReadOnlyList<string> keys, IReadOnlyList<string> arguments,
        object data, CancellationToken cancellationToken);

    /// <summary>
    /// Прочитать следующую порцию типизированных записей журнала после указанной позиции.
    /// </summary>
    Task<IReadOnlyList<RedisStreamEntry<T>>> ReadStreamAsync<T>(string stream, string field, string position,
        int count, CancellationToken cancellationToken);
}
