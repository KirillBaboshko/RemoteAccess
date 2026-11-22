using MediatR;
using RemoteAccess.Signaling.Application.Models;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public record RegisterHostCommand(SignalingCaller Caller, Guid DeviceId) : IRequest<Unit>;
