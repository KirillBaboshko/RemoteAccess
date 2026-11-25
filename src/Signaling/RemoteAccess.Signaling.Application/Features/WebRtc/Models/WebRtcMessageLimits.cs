namespace RemoteAccess.Signaling.Application.Features.WebRtc.Models;

public static class WebRtcMessageLimits
{
    public const int SdpBytes = 64 * 1024;
    public const int IceCandidateBytes = 4 * 1024;
    public const int IceFieldLength = 256;
    public const int HubMessageBytes = 128 * 1024;
}
