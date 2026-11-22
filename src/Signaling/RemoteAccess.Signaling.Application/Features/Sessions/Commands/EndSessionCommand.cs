using MediatR;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public record EndSessionCommand(SignalingCaller Caller, Guid SessionId) : IRequest<SessionModel>;
