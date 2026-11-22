using MediatR;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public record ExpireSessionsCommand : IRequest<Unit>;
