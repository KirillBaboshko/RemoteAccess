using FluentValidation;
using RemoteAccess.Common.Redis;
using RemoteAccess.Signaling.Application.Features.Sessions.Models;

namespace RemoteAccess.Signaling.Infrastructure.Settings;

public sealed class InfrastructureSettings
{
    public RedisClientSettings RedisSettings { get; init; } = new();
    public SessionOptions SessionSettings { get; init; } = new();
    public CoreApiSettings CoreSettings { get; init; } = new();
    public string DatabaseName { get; init; } = "RemoteAccessSignalingDb";

    internal void Validate() => new InfrastructureSettingsValidator().ValidateAndThrow(this);
}
