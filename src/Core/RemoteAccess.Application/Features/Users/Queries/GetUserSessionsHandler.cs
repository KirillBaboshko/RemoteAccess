using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Sessions;
using RemoteAccess.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public sealed class GetUserSessionsHandler : IRequestHandler<UserSessionsQuery, PagedResult<SessionHistoryModel>>
{
    private readonly ISessionHistoryProvider _history;

    public GetUserSessionsHandler(ISessionHistoryProvider history) => _history = history;

    public Task<PagedResult<SessionHistoryModel>> Handle(UserSessionsQuery request, CancellationToken cancellationToken) =>
        _history.GetHistory(request.Filter, cancellationToken);
}
