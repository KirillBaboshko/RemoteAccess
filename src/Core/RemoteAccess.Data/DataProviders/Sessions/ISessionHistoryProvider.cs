using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Sessions.Model;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Data.DataProviders.Sessions;

public interface ISessionHistoryProvider
{
    Task<PagedResult<SessionHistoryModel>> GetHistory(SessionHistoryFilter filter, CancellationToken cancellationToken);
    Task<SessionHistoryModel?> GetById(Guid sessionId, CancellationToken cancellationToken);
}

public sealed class SessionHistoryUnavailableException(Exception innerException)
    : Exception(ValidationErrorMessages.HistoryUnavailable, innerException);
