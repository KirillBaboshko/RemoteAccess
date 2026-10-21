using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Domain.Models.Devices
{
    public class Device
    {
        public Guid Id { get; private set; }
        public Guid OwnerId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public PlatformType PlatformType { get; private set; }
        public DeviceStatus Status { get; private set; }
        public bool IsCanConnected => Status == DeviceStatus.Online;
        public Device(){}
        public Device(Guid ownerId, string name, PlatformType platformType)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Name = name;
            PlatformType = platformType;
            Status = DeviceStatus.Online;
        }
        
        public void DeviceOn()=>Status = DeviceStatus.Online;
        public void DeviceOff()=>Status = DeviceStatus.Offline;

    }
}
