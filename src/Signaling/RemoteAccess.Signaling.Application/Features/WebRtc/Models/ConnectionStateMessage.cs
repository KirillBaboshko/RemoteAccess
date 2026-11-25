using RemoteAccess.Signaling.Domain.Models.WebRtc;

namespace RemoteAccess.Signaling.Application.Features.WebRtc.Models;

public record ConnectionStateMessage(Guid SessionId, WebRtcConnectionState State);
