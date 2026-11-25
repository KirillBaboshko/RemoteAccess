using MediatR;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public record GetSessionQuery(SignalingCaller Caller, Guid SessionId) : IRequest<SessionModel>;
