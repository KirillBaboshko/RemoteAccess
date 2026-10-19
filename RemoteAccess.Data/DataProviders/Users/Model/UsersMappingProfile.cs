using AutoMapper;
using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Data.DataProviders.Users.Model
{
    public class UsersMappingProfile : Profile
    {
        /// <inheritdoc />
        public UsersMappingProfile()
        {
            CreateMap<User, UserShortModel>()
                .ForMember(a => a.Id, e => e.MapFrom(a => a.Id));

            CreateMap<User, UserModel>()
                .ForMember(a => a.Id, e => e.MapFrom(a => a.Id));

        }
    }
}
