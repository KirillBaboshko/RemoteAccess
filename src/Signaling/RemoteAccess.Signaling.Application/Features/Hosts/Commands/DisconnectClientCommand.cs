using MediatR;

namespace RemoteAccess.Signaling.Application.Features.Hosts.Commands;

public record DisconnectClientCommand(string ConnectionId) : IRequest<Unit>;
