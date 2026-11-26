using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using RemoteAccess.Signaling.Clients.Ice;
using RemoteAccess.Signaling.Clients.Ice.Models;
using RemoteAccess.Signaling.Infrastructure.Settings;

namespace RemoteAccess.Signaling.Infrastructure.Clients;

public sealed class CoturnIceConfigurationProvider : IIceConfigurationProvider
{
    private readonly IceSettings _settings;
    private readonly TimeProvider _clock;

    public CoturnIceConfigurationProvider(IOptions<IceSettings> settings, TimeProvider clock)
    {
        _settings = settings.Value;
        _clock = clock;
    }

    public IceConfigurationModel GetConfiguration(Guid userId)
    {
        var servers = new List<IceServerModel>();
        if (_settings.StunUrls.Length > 0) servers.Add(new(_settings.StunUrls.ToArray()));
        DateTime? expiresAt = null;
        if (_settings.TurnUrls.Length > 0)
        {
            var timestamp = _clock.GetUtcNow().Add(_settings.CredentialLifetime).ToUnixTimeSeconds();
            var username = timestamp.ToString(CultureInfo.InvariantCulture) + ":" + userId.ToString("D");
            var credential = Convert.ToBase64String(HMACSHA1.HashData(
                Encoding.UTF8.GetBytes(_settings.SharedSecret), Encoding.UTF8.GetBytes(username)));
            servers.Add(new(_settings.TurnUrls.ToArray(), username, credential));
            expiresAt = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        }
        return new(servers.ToArray(), expiresAt);
    }
}
