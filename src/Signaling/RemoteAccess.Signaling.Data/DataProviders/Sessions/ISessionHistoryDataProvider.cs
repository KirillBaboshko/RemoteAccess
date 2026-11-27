using RemoteAccess.Common.Data;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Signaling.Data.DataProviders.Sessions;

public interface ISessionHistoryDataProvider
{
    Task<PagedResult<SessionHistoryModel>> GetHistory(Guid userId, SessionHistoryFilter filter,
        CancellationToken cancellationToken);
    Task<SessionHistoryModel?> GetById(Guid userId, Guid sessionId, CancellationToken cancellationToken);
}
