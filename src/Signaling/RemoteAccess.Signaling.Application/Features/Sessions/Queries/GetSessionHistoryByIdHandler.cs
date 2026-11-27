using MediatR;
using RemoteAccess.Signaling.Data.DataProviders.Sessions;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class GetSessionHistoryByIdHandler : IRequestHandler<SessionHistoryByIdQuery, SessionHistoryModel?>
{
    private readonly ISessionHistoryDataProvider _history;

    public GetSessionHistoryByIdHandler(ISessionHistoryDataProvider history) => _history = history;

    public Task<SessionHistoryModel?> Handle(SessionHistoryByIdQuery request, CancellationToken cancellationToken) =>
        _history.GetById(request.UserId, request.SessionId, cancellationToken);
}
