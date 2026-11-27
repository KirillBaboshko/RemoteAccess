using MediatR;
using RemoteAccess.Common.Data;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public record SessionHistoryQuery(Guid UserId, SessionHistoryFilter Filter) : IRequest<PagedResult<SessionHistoryModel>>;
