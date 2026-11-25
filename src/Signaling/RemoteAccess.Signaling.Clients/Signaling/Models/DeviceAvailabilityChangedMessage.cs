namespace RemoteAccess.Signaling.Clients.Signaling.Models;

/// <summary>
/// Сигнал перечитать устройства из Core. Revision — идентификатор записи Redis Stream.
/// </summary>
public record DeviceAvailabilityChangedMessage(Guid[] DeviceIds, string Revision);
