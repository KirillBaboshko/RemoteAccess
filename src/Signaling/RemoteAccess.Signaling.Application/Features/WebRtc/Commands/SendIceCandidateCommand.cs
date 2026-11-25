using MediatR;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Application.Models;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public record SendIceCandidateCommand(SignalingCaller Caller, IceCandidateMessage Candidate) : IRequest<Unit>;
