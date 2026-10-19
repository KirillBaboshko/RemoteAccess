namespace RemoteAccess.Api.Signaling;

public interface ISignalingClient
{
    Task UsersUpdated(IReadOnlyList<OnlineUserDto> users);
    Task IncomingInvite(Guid fromUserId, string fromEmail);
    Task InviteAccepted(Guid peerUserId, string peerEmail);
    Task InviteDeclined(Guid peerUserId);
    Task Offer(Guid fromUserId, string sdp);
    Task Answer(Guid fromUserId, string sdp);
    Task IceCandidate(Guid fromUserId, string candidateJson);
    Task PeerLeft(Guid userId);
}
