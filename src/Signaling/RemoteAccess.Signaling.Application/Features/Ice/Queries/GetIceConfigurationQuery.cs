using MediatR;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Clients.Ice.Models;

namespace RemoteAccess.Signaling.Application.Features.Ice.Queries;

public record GetIceConfigurationQuery(SignalingCaller Caller) : IRequest<IceConfigurationModel>;
