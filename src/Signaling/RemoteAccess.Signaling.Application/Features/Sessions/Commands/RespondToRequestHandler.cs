using AutoMapper;
using MediatR;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RespondToRequestHandler : IRequestHandler<RespondToRequestCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;
    private readonly IMapper _mapper;

    public RespondToRequestHandler(ISignalingState state, ISignalingNotifier notifier, IMapper mapper)
    {
        _state = state;
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task<SessionModel> Handle(RespondToRequestCommand request, CancellationToken cancellationToken)
    {
        var session = await _state.RespondAsync(request.SessionId, request.Caller.ConnectionId, request.Accepted, cancellationToken);
        await _notifier.SessionUpdated(session);
        return _mapper.Map<SessionModel>(session);
    }
}
