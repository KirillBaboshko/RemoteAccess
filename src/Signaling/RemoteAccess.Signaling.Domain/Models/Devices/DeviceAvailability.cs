namespace RemoteAccess.Signaling.Domain.Models.Devices;

public record DeviceAvailability(Guid DeviceId, DeviceConnectionStatus Status);
