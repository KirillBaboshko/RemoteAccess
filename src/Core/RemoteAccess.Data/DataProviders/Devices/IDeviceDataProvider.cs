using RemoteAccess.Data.DataProviders.Devices.Model;

namespace RemoteAccess.Data.DataProviders.Devices
{
    public interface IDeviceDataProvider
    {
        Task<List<DeviceModel>> GetDevicesByOwnerId(Guid ownerId, CancellationToken cancellationToken);
        Task<List<DeviceModel>> SearchDevices(string search, CancellationToken cancellationToken);
    }
}
