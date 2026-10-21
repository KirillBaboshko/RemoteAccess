using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Application.Features.Devices.Models
{
    public class DeviceInfo
    {
        public Guid OwnerId { get; init; }
        public string Name { get; init; } = string.Empty;
        public PlatformType PlatformType { get; init; }

    }
}
