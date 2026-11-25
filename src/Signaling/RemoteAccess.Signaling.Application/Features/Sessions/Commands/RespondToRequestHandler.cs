using AutoMapper;
using MediatR;
using RemoteAccess.Signaling.Clients.Devices;
using RemoteAccess.Signaling.Domain.Models.Sessions;
using RemoteAccess.Signaling.Domain.Repositories;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Application.Features.Sessions.Commands;

public sealed class RespondToRequestHandler : IRequestHandler<RespondToRequestCommand, SessionModel>
{
    private readonly ISignalingState _state;
    private readonly IDeviceAccessProvider _devices;
    private readonly IMapper _mapper;

    public RespondToRequestHandler(ISignalingState state, IDeviceAccessProvider devices, IMapper mapper)
    {
        _state = state;
        _devices = devices;
        _mapper = mapper;
    }

    public async Task<SessionModel> Handle(RespondToRequestCommand request, CancellationToken cancellationToken)
    {
        var current = await _state.GetSessionAsync(request.SessionId, request.Caller.UserId, cancellationToken);
        if (request.Accepted && current.State == SessionState.Pending)
        {
            foreach (var deviceId in new[] { current.Source.DeviceId, current.Target.DeviceId })
            {
                var access = await _devices.GetAccess(deviceId, request.Caller.AccessToken, cancellationToken);
                await _state.UpdateDeviceStatusAsync(deviceId, access?.IsOnline == true, cancellationToken);
            }
        }
        var session = await _state.RespondAsync(request.SessionId, request.Caller.ConnectionId, request.Accepted, cancellationToken);
        return _mapper.Map<SessionModel>(session);
    }
}
