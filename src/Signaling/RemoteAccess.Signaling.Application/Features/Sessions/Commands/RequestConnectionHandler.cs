using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RequestConnectionHandler : IRequestHandler<RequestConnectionCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly ISignalingNotifier _notifier;
    private readonly IMapper _mapper;
    private readonly SessionOptions _options;

    public RequestConnectionHandler(ISignalingState state, ISignalingNotifier notifier, IMapper mapper,
        IOptions<SessionOptions> options)
    {
        _state = state;
        _notifier = notifier;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<SessionModel> Handle(RequestConnectionCommand request, CancellationToken cancellationToken)
    {
        var source = new SessionParticipant(request.Caller.UserId, request.Request.SourceDeviceId, request.Caller.ConnectionId);
        var session = await _state.CreateSessionAsync(source, request.Request.TargetDeviceId, _options.PendingTimeout, cancellationToken);
        try
        {
            await _notifier.IncomingRequest(session);
        }
        catch
        {
            try
            {
                await _state.EndAsync(session.Id, source.ConnectionId, CancellationToken.None);
            }
            catch (SignalingException)
            {
            }
            throw;
        }
        return _mapper.Map<SessionModel>(session);
    }
}
