using RemoteAccess.Common.Data;
using RemoteAccess.Infrastructure.Database;

namespace RemoteAccess.Infrastructure.Data;

/// <summary>
/// Реализация Unit of Work для управления транзакциями
/// </summary>
public class UnitOfWork : Common.Data.UnitOfWork
{
    public UnitOfWork(AppDbContext dbContext) : base(dbContext)
    {
    }
}
