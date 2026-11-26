using RemoteAccess.Signaling.Clients.Ice.Models;

namespace RemoteAccess.Signaling.Clients.Ice;

public interface IIceConfigurationProvider
{
    IceConfigurationModel GetConfiguration(Guid userId);
}
