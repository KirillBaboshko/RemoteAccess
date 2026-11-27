using MediatR;
using RemoteAccess.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public sealed record UserSessionByIdQuery(Guid SessionId) : IRequest<SessionHistoryModel?>;
