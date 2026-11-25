namespace RemoteAccess.Signaling.Clients.Signaling.Models;

public record IceCandidateMessage(Guid SessionId, string? Candidate, string? SdpMid = null,
    int? SdpMLineIndex = null, string? UsernameFragment = null);
