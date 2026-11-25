using MediatR;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using RemoteAccess.Signaling.Application.Models;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public record ReportConnectionStateCommand(SignalingCaller Caller, ConnectionStateMessage Message) : IRequest<SessionModel>;
