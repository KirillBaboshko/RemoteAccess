using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Domain.Repositories;

public interface ISessionHistoryRepository
{
    Task SaveAsync(RemoteSession session, CancellationToken cancellationToken);
}
