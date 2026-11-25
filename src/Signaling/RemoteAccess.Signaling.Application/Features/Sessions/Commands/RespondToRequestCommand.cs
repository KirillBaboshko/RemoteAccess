using MediatR;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public record RespondToRequestCommand(SignalingCaller Caller, Guid SessionId, bool Accepted) : IRequest<SessionModel>;
