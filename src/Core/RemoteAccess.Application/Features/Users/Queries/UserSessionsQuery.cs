using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public sealed record UserSessionsQuery(SessionHistoryFilter Filter) : IRequest<PagedResult<SessionHistoryModel>>;
