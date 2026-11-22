using RemoteAccess.Common.Data;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Infrastructure.Database;

namespace RemoteAccess.Signaling.Infrastructure.Repositories;

public sealed class SessionHistoryRepository : RepositoryBase<SessionHistory, SessionId, Guid>, ISessionHistoryRepository
{
    public SessionHistoryRepository(SignalingDbContext context) : base(context) { }

    public async Task SaveAsync(RemoteSession session, CancellationToken cancellationToken)
    {
        var history = await InternalGetByIdAsync(new SessionId(session.Id), cancellationToken);
        if (history is null)
            InternalCreate(new SessionHistory(session));
        else
            history.Apply(session);
    }
}
