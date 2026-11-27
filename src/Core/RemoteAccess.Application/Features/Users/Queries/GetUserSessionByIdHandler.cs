using MediatR;
using RemoteAccess.Data.DataProviders.Sessions;
using RemoteAccess.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public sealed class GetUserSessionByIdHandler : IRequestHandler<UserSessionByIdQuery, SessionHistoryModel?>
{
    private readonly ISessionHistoryProvider _history;

    public GetUserSessionByIdHandler(ISessionHistoryProvider history) => _history = history;

    public Task<SessionHistoryModel?> Handle(UserSessionByIdQuery request, CancellationToken cancellationToken) =>
        _history.GetById(request.SessionId, cancellationToken);
}
