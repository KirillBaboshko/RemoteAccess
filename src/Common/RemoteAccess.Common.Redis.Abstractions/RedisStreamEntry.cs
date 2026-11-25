namespace RemoteAccess.Common.Redis.Abstractions;

/// <summary>
/// Value отсутствует у повреждённой записи; её Id позволяет читателю продолжить журнал.
/// </summary>
public record RedisStreamEntry<T>(string Id, T? Value) where T : class;
