using MediatR;
using RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public record SessionHistoryByIdQuery(Guid UserId, Guid SessionId) : IRequest<SessionHistoryModel?>;
