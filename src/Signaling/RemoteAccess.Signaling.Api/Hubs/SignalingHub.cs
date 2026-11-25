using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RemoteAccess.Signaling.Application.Features.Hosts.Commands;
using RemoteAccess.Signaling.Application.Features.Sessions.Commands;
using RemoteAccess.Signaling.Application.Models;
using RemoteAccess.Signaling.Domain.Exceptions;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;
using RemoteAccess.Signaling.Application.Features.WebRtc.Commands;
using RemoteAccess.Signaling.Application.Features.WebRtc.Models;
using RemoteAccess.Signaling.Clients.Signaling;
using RemoteAccess.Signaling.Clients.Signaling.Models;

namespace RemoteAccess.Signaling.Api.Hubs;

[Authorize]
public sealed class SignalingHub : Hub<ISignalingClient>
{
    private readonly ISender _sender;

    public SignalingHub(ISender sender) => _sender = sender;

    public override async Task OnConnectedAsync()
    {
        await _sender.Send(new ConnectClientCommand(GetCaller()), Context.ConnectionAborted);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            await _sender.Send(new DisconnectClientCommand(Context.ConnectionId));
        }
        finally
        {
            await base.OnDisconnectedAsync(exception);
        }
    }

    public async Task RegisterHost(Guid deviceId) =>
        await _sender.Send(new RegisterHostCommand(GetCaller(), deviceId), Context.ConnectionAborted);

    public Task<SessionModel> RequestConnection(ConnectionRequest request) =>
        _sender.Send(new RequestConnectionCommand(GetCaller(), request), Context.ConnectionAborted);

    public Task<SessionModel> RespondToRequest(Guid sessionId, bool accepted) =>
        _sender.Send(new RespondToRequestCommand(GetCaller(), sessionId, accepted), Context.ConnectionAborted);

    public Task<SessionModel> EndSession(Guid sessionId) =>
        _sender.Send(new EndSessionCommand(GetCaller(), sessionId), Context.ConnectionAborted);

    public async Task SendOffer(SessionDescriptionMessage offer) =>
        await _sender.Send(new SendOfferCommand(GetCaller(), offer), Context.ConnectionAborted);

    public async Task SendAnswer(SessionDescriptionMessage answer) =>
        await _sender.Send(new SendAnswerCommand(GetCaller(), answer), Context.ConnectionAborted);

    public async Task SendIceCandidate(IceCandidateMessage candidate) =>
        await _sender.Send(new SendIceCandidateCommand(GetCaller(), candidate), Context.ConnectionAborted);

    public Task<SessionModel> ReportConnectionState(ConnectionStateMessage message) =>
        _sender.Send(new ReportConnectionStateCommand(GetCaller(), message), Context.ConnectionAborted);

    private SignalingCaller GetCaller()
    {
        if (!Guid.TryParse(Context.User?.FindFirst("userId")?.Value, out var userId) || userId == Guid.Empty)
            throw new HubException(ValidationErrorMessages.UserRequired);

        var http = Context.GetHttpContext();
        var header = http?.Request.Headers.Authorization.ToString();
        var token = header?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
            ? header[7..]
            : http?.Request.Query["access_token"].ToString();
        if (string.IsNullOrWhiteSpace(token))
            throw new HubException(ValidationErrorMessages.TokenRequired);

        return new SignalingCaller(userId, Context.ConnectionId, token);
    }
}
