using MediatR;
using RemoteAccess.Signaling.Application.Models;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public record ConnectClientCommand(SignalingCaller Caller) : IRequest<Unit>;
