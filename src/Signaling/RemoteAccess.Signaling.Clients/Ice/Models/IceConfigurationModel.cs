namespace RemoteAccess.Signaling.Clients.Ice.Models;

public record IceConfigurationModel(IceServerModel[] IceServers, DateTime? ExpiresAt);
