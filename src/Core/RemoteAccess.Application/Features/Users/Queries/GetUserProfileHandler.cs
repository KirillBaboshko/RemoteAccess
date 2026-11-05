using MediatR;
using RemoteAccess.Data.DataProviders.Users;
using RemoteAccess.Data.DataProviders.Users.Model;

namespace RemoteAccess.Application.Features.Users.Queries;

public class GetUserProfileHandler : IRequestHandler<UserProfileQuery, UserModel>
{
    private readonly IUserDataProvider _usersDataProvider;

    public GetUserProfileHandler(IUserDataProvider usersDataProvider)
    {
        _usersDataProvider = usersDataProvider;
    }

    public async Task<UserModel> Handle(UserProfileQuery request, CancellationToken cancellationToken)
    {
        return await _usersDataProvider.GetUserById(request.UserId, cancellationToken);
    }
}
