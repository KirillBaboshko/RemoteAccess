using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Signaling.Data.DataProviders.Sessions;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class GetSessionHistoryHandler : IRequestHandler<SessionHistoryQuery, PagedResult<SessionHistoryModel>>
{
    private readonly ISessionHistoryDataProvider _history;

    public GetSessionHistoryHandler(ISessionHistoryDataProvider history) => _history = history;

    public Task<PagedResult<SessionHistoryModel>> Handle(SessionHistoryQuery request, CancellationToken cancellationToken) =>
        _history.GetHistory(request.UserId, request.Filter, cancellationToken);
}
