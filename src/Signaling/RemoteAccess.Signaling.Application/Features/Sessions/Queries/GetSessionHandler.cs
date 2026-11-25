using AutoMapper;
using MediatR;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Domain.Repositories;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Queries;

public sealed class GetSessionHandler : IRequestHandler<GetSessionQuery, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly IMapper _mapper;

    public GetSessionHandler(ISignalingState state, IMapper mapper)
    {
        _state = state;
        _mapper = mapper;
    }

    public async Task<SessionModel> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _state.GetSessionAsync(request.SessionId, request.Caller.UserId, cancellationToken);
        return _mapper.Map<SessionModel>(session);
    }
}
