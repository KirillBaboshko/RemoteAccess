using AutoMapper;
using MediatR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class EndSessionHandler : IRequestHandler<EndSessionCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;
    private readonly IMapper _mapper;

    public EndSessionHandler(ISignalingState state, ISignalingNotifier notifier, IMapper mapper)
    {
        _state = state;
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task<SessionModel> Handle(EndSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.EndAsync(request.SessionId, request.Caller.ConnectionId, cancellationToken);
        await _notifier.SessionUpdated(session);
        return _mapper.Map<SessionModel>(session);
    }
}
