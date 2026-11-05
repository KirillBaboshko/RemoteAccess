namespace RemoteAccess.Common.Data;

/// <summary>
/// Интерфейс Unit of Work для управления транзакциями
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохранение всех изменений в базе данных
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Количество затронутых записей</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Начало транзакции
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Подтверждение транзакции
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Откат транзакции
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
