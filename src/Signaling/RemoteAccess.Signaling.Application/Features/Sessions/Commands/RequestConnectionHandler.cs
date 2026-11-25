using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using RemoteAccess.Signaling.Clients.Signaling.Models;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RequestConnectionHandler : IRequestHandler<RequestConnectionCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly IMapper _mapper;
    private readonly SessionOptions _options;
    private readonly IDeviceAccessProvider _devices;

    public RequestConnectionHandler(ISignalingState state, IDeviceAccessProvider devices, IMapper mapper,
        IOptions<SessionOptions> options)
    {
        _state = state;
        _mapper = mapper;
        _options = options.Value;
        _devices = devices;
    }

    public async Task<SessionModel> Handle(RequestConnectionCommand request, CancellationToken cancellationToken)
    {
        foreach (var deviceId in new[] { request.Request.SourceDeviceId, request.Request.TargetDeviceId })
        {
            var access = await _devices.GetAccess(deviceId, request.Caller.AccessToken, cancellationToken);
            await _state.UpdateDeviceStatusAsync(deviceId, access?.IsOnline == true, cancellationToken);
        }
        var source = new SessionParticipant(request.Caller.UserId, request.Request.SourceDeviceId, request.Caller.ConnectionId);
        var session = await _state.CreateSessionAsync(source, request.Request.TargetDeviceId, _options.PendingTimeout, cancellationToken);
        return _mapper.Map<SessionModel>(session);
    }
}
