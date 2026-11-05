using System.Linq.Expressions;

namespace RemoteAccess.Common.Extensions;

/// <summary>
/// Расширения для IQueryable
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Применяет условие Where только если предикат истинен
    /// </summary>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }

    /// <summary>
    /// Применяет сортировку только если условие истинно
    /// </summary>
    public static IQueryable<T> OrderByIf<T, TKey>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, TKey>> keySelector,
        bool descending = false)
    {
        if (!condition)
            return query;

        return descending
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);
    }
}
