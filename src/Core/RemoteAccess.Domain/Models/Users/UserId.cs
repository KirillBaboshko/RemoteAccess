using RemoteAccess.Common.Domain;

namespace RemoteAccess.Domain.Models.Users;

/// <summary>
/// Strongly-typed идентификатор пользователя
/// </summary>
public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    /// <summary>
    /// Создание нового идентификатора
    /// </summary>
    public static UserId New() => new(Guid.NewGuid());

    /// <summary>
    /// Создание из строки
    /// </summary>
    public static UserId FromString(string value) => new(Guid.Parse(value));
}
