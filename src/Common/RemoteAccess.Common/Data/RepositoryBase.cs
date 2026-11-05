using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RemoteAccess.Common.Domain;

namespace RemoteAccess.Common.Data;

/// <summary>
/// Базовый репозиторий для работы с сущностями
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TStronglyTypedId">Тип strongly-typed идентификатора</typeparam>
/// <typeparam name="TId">Базовый тип идентификатора</typeparam>
public abstract class RepositoryBase<TEntity, TStronglyTypedId, TId>
    where TId : IComparable<TId>, IComparable
    where TStronglyTypedId : StronglyTypedId<TId>
    where TEntity : Entity<TStronglyTypedId, TId>
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Конструктор базового репозитория
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    protected RepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
        Database = _dbContext.Database;
    }

    /// <summary>
    /// Доступ к Database Facade для транзакций и raw SQL
    /// </summary>
    protected DatabaseFacade Database { get; }

    /// <summary>
    /// Базовый queryable для работы с сущностями
    /// </summary>
    protected virtual IQueryable<TEntity> Query => _dbSet.AsQueryable();

    /// <summary>
    /// Добавление сущности в контекст (без сохранения)
    /// </summary>
    /// <param name="entity">Сущность для добавления</param>
    protected virtual void InternalCreate(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Add(entity);
    }

    /// <summary>
    /// Добавление множества сущностей в контекст (без сохранения)
    /// </summary>
    /// <param name="entities">Коллекция сущностей</param>
    protected virtual void InternalCreateRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.AddRange(entities);
    }

    /// <summary>
    /// Обновление сущности в контексте (без сохранения)
    /// </summary>
    /// <param name="entity">Сущность для обновления</param>
    protected virtual void InternalUpdate(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Удаление сущности из контекста (без сохранения)
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    protected virtual void InternalDelete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Удаление множества сущностей из контекста (без сохранения)
    /// </summary>
    /// <param name="entities">Коллекция сущностей для удаления</param>
    protected virtual void InternalDeleteRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность или null</returns>
    protected virtual async Task<TEntity?> InternalGetByIdAsync(TStronglyTypedId id, CancellationToken cancellationToken = default)
    {
        return await Query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Проверка существования сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>True если существует</returns>
    protected virtual async Task<bool> InternalExistsAsync(TStronglyTypedId id, CancellationToken cancellationToken = default)
    {
        return await Query.AnyAsync(e => e.Id.Equals(id), cancellationToken);
    }
}
