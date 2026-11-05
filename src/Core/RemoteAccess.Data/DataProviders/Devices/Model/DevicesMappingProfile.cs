using AutoMapper;
using RemoteAccess.Domain.Models.Devices;

namespace RemoteAccess.Data.DataProviders.Devices.Model;

/// <summary>
/// Профиль преобразований для <see cref="Device"/>
/// </summary>
public class DevicesMappingProfile : Profile
{
    /// <inheritdoc />
    public DevicesMappingProfile()
    {
        CreateMap<Device, DeviceModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.PlatformType, opt => opt.MapFrom(src => src.PlatformType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
