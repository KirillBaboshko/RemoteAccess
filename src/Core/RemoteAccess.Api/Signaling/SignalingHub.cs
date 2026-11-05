using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RemoteAccess.Api.Signaling;

[Authorize]
public class SignalingHub : Hub<ISignalingClient>
{
    private readonly IPresenceTracker _presence;

    public SignalingHub(IPresenceTracker presence)
    {
        _presence = presence;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        var email = GetEmail();
        _presence.Add(userId, email, Context.ConnectionId);
        await Clients.All.UsersUpdated(_presence.GetOnlineUsers());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        _presence.Remove(Context.ConnectionId);
        await Clients.Others.PeerLeft(userId);
        await Clients.All.UsersUpdated(_presence.GetOnlineUsers());
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Invite(Guid targetUserId)
    {
        var fromUserId = GetUserId();
        EnsureNotSelf(targetUserId, fromUserId);
        await SendToUser(targetUserId, client => client.IncomingInvite(fromUserId, GetEmail()));
    }

    public async Task AcceptInvite(Guid callerUserId)
    {
        var hostUserId = GetUserId();
        EnsureNotSelf(callerUserId, hostUserId);
        await SendToUser(callerUserId, client => client.InviteAccepted(hostUserId, GetEmail()));
    }

    public async Task DeclineInvite(Guid callerUserId)
    {
        var hostUserId = GetUserId();
        EnsureNotSelf(callerUserId, hostUserId);
        await SendToUser(callerUserId, client => client.InviteDeclined(hostUserId));
    }

    public Task SendOffer(Guid targetUserId, string sdp) =>
        Relay(targetUserId, client => client.Offer(GetUserId(), sdp));

    public Task SendAnswer(Guid targetUserId, string sdp) =>
        Relay(targetUserId, client => client.Answer(GetUserId(), sdp));

    public Task SendIceCandidate(Guid targetUserId, string candidateJson) =>
        Relay(targetUserId, client => client.IceCandidate(GetUserId(), candidateJson));

    public Task Hangup(Guid targetUserId) =>
        Relay(targetUserId, client => client.PeerLeft(GetUserId()));

    private async Task Relay(Guid targetUserId, Func<ISignalingClient, Task> send)
    {
        EnsureNotSelf(targetUserId, GetUserId());
        await SendToUser(targetUserId, send);
    }

    private async Task SendToUser(Guid targetUserId, Func<ISignalingClient, Task> send)
    {
        var connections = _presence.GetConnectionIds(targetUserId);
        if (connections.Count == 0)
        {
            throw new HubException("Пользователь не в сети");
        }

        foreach (var connectionId in connections)
        {
            await send(Clients.Client(connectionId));
        }
    }

    private Guid GetUserId()
    {
        var value = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? Context.User?.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(value, out var userId))
        {
            throw new HubException("Пользователь не авторизован");
        }

        return userId;
    }

    private string GetEmail() =>
        Context.User?.FindFirstValue(ClaimTypes.Email)
        ?? Context.User?.Identity?.Name
        ?? string.Empty;

    private static void EnsureNotSelf(Guid targetUserId, Guid currentUserId)
    {
        if (targetUserId == currentUserId)
        {
            throw new HubException("Нельзя подключиться к самому себе");
        }
    }
}
