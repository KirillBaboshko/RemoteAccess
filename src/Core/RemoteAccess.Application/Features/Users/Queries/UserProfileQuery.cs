using MediatR;
using RemoteAccess.Data.DataProviders.Users.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public class UserProfileQuery : IRequest<UserModel>
{
    public Guid UserId { get; init; }

    public UserProfileQuery(Guid userId)
    {
        UserId = userId;
    }
}
