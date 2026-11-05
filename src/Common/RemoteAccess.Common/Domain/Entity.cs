namespace RemoteAccess.Common.Domain;

/// <summary>
/// Базовая сущность для Domain Driven Design
/// </summary>
/// <typeparam name="TStronglyTypedId">Тип strongly-typed идентификатора</typeparam>
/// <typeparam name="TId">Базовый тип идентификатора (Guid, int, etc.)</typeparam>
public abstract class Entity<TStronglyTypedId, TId>
    where TId : IComparable<TId>, IComparable
    where TStronglyTypedId : StronglyTypedId<TId>
{
    /// <summary>
    /// Уникальный идентификатор сущности
    /// </summary>
    public TStronglyTypedId Id { get; protected set; } = default!;

    /// <summary>
    /// Дата создания сущности
    /// </summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего обновления сущности
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Помечает сущность как обновлённую
    /// </summary>
    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Сравнение сущностей по идентификатору
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TStronglyTypedId, TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (Id == null || other.Id == null)
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// Получение хеш-кода на основе идентификатора
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    /// Оператор сравнения
    /// </summary>
    public static bool operator ==(Entity<TStronglyTypedId, TId>? left, Entity<TStronglyTypedId, TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Оператор неравенства
    /// </summary>
    public static bool operator !=(Entity<TStronglyTypedId, TId>? left, Entity<TStronglyTypedId, TId>? right)
    {
        return !(left == right);
    }
}
