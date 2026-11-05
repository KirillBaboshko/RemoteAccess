using Microsoft.EntityFrameworkCore;

namespace RemoteAccess.Common.Data;

/// <summary>
/// Базовый класс для провайдеров данных (CQRS - Read Side)
/// </summary>
public abstract class DataProviderBase
{
    private readonly DbContext _dbContext;

    /// <summary>
    /// Конструктор базового провайдера данных
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    protected DataProviderBase(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Получение queryable для чтения данных с AsNoTracking
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <returns>IQueryable для чтения</returns>
    protected IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return _dbContext.Set<TEntity>().AsNoTracking();
    }
}
