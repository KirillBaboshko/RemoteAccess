using RemoteAccess.Domain.Models.Devices;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Data.DataProviders.Devices;

public interface IDeviceAvailabilityProvider
{
    Task<IReadOnlyDictionary<Guid, DeviceStatus>> GetAvailability(
        IReadOnlyCollection<Guid> deviceIds, CancellationToken cancellationToken);
    Task NotifyStatusChanged(Guid deviceId, CancellationToken cancellationToken);
}

public sealed class DeviceAvailabilityUnavailableException : Exception
{
    public DeviceAvailabilityUnavailableException(Exception? innerException = null)
        : base(ValidationErrorMessages.DeviceAvailabilityUnavailable, innerException) { }
}
