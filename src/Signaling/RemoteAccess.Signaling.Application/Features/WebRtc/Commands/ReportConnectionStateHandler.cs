using AutoMapper;
using MediatR;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Commands;

public sealed class ReportConnectionStateHandler : IRequestHandler<ReportConnectionStateCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;
    private readonly IMapper _mapper;

    public ReportConnectionStateHandler(ISignalingState state, ISignalingNotifier notifier, IMapper mapper)
    {
        _state = state;
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task<SessionModel> Handle(ReportConnectionStateCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.ReportConnectionStateAsync(request.Message.SessionId,
            request.Caller.UserId, request.Caller.ConnectionId, request.Message.State, cancellationToken);
        await _notifier.SessionUpdated(session);
        return _mapper.Map<SessionModel>(session);
    }
}
