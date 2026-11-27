using AutoMapper;
using RemoteAccess.Signaling.Domain.Models.Sessions;

namespace RemoteAccess.Signaling.Data.DataProviders.Sessions.Model;

public sealed class SessionHistoryMappingProfile : Profile
{
    public SessionHistoryMappingProfile()
    {
        CreateMap<SessionHistory, SessionHistoryModel>()
            .ForMember(x => x.SessionId, options => options.MapFrom(x => x.Id.Value));
    }
}
