using RemoteAccess.Signaling.Domain.Exceptions;

namespace RemoteAccess.Signaling.Infrastructure.Redis.Models;

public sealed record RedisStateResult(string? Error, RedisSessionRecord? Session, RedisSessionRecord[]? Sessions)
{
    public void EnsureSuccess()
    {
        if (Error is null) return;
        var message = Error switch
        {
            "ConnectionRequired" => ValidationErrorMessages.ConnectionRequired,
            "ConnectionAlreadyRegistered" => ValidationErrorMessages.ConnectionAlreadyRegistered,
            "HostAlreadyRegistered" => ValidationErrorMessages.HostAlreadyRegistered,
            "SelfConnection" => ValidationErrorMessages.SelfConnection,
            "DeviceOffline" => ValidationErrorMessages.DeviceOffline,
            "DeviceBusy" => ValidationErrorMessages.DeviceBusy,
            "RequestUnavailable" => ValidationErrorMessages.RequestUnavailable,
            "SessionUnavailable" => ValidationErrorMessages.SessionUnavailable,
            _ => ValidationErrorMessages.UnexpectedError
        };
        throw new SignalingException(message);
    }
}
