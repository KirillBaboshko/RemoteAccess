using MediatR;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public record RequestConnectionCommand(SignalingCaller Caller, ConnectionRequest Request) : IRequest<SessionModel>;
