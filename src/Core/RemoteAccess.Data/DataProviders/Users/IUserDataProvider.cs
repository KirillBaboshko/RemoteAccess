using RemoteAccess.Data.DataProviders.Users.Model;

namespace RemoteAccess.Data.DataProviders.Users
{
    public interface IUserDataProvider
    {
        public Task<UserModel> GetUserById(Guid id, CancellationToken cancellationToken);
        
    }
}
