namespace RemoteAccess.Signaling.Application.Features.Sessions.Models;

public sealed class SessionOptions
{
    public TimeSpan PendingTimeout { get; set; } = TimeSpan.FromMinutes(1);
    public TimeSpan NegotiationTimeout { get; set; } = TimeSpan.FromMinutes(1);
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan RecoveryTimeout { get; set; } = TimeSpan.FromSeconds(15);
    public TimeSpan ConnectionLease { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan SessionLease { get; set; } = TimeSpan.FromMinutes(2);
    public TimeSpan CleanupGrace { get; set; } = TimeSpan.FromMinutes(1);
    public TimeSpan MaintenanceInterval { get; set; } = TimeSpan.FromSeconds(5);
}
