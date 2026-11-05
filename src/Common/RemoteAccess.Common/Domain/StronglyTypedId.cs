namespace RemoteAccess.Common.Domain;

/// <summary>
/// Базовый класс для strongly-typed идентификаторов
/// </summary>
/// <typeparam name="TValue">Тип значения идентификатора</typeparam>
public abstract record StronglyTypedId<TValue>(TValue Value)
    where TValue : IComparable<TValue>, IComparable
{
    /// <summary>
    /// Неявное преобразование к базовому типу
    /// </summary>
    public static implicit operator TValue(StronglyTypedId<TValue> id) => id.Value;

    /// <summary>
    /// Переопределение ToString для удобного отображения
    /// </summary>
    public override string ToString() => Value?.ToString() ?? string.Empty;
}
