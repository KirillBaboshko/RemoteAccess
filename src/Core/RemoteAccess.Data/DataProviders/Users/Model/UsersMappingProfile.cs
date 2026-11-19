using AutoMapper;
using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Data.DataProviders.Users.Model
{
    /// <summary>
    /// Профиль преобразований для <see cref="User"/>
    /// </summary>
    public class UsersMappingProfile : Profile
    {
        /// <inheritdoc />
        public UsersMappingProfile()
        {
            CreateMap<User, UserShortModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
