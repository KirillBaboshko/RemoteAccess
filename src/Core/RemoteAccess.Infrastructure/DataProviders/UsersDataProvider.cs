using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteAccess.Data.DataProviders.Users;
using RemoteAccess.Data.DataProviders.Users.Model;
using RemoteAccess.Domain.Models.Users;
using RemoteAccess.Infrastructure.Database;

namespace RemoteAccess.Infrastructure.DataProviders;

/// <summary>
/// Провайдер данных для чтения информации о пользователях
/// </summary>
public class UsersDataProvider : IUserDataProvider
{
    private readonly DbSet<User> _users;
    private readonly IMapper _mapper;

    public UsersDataProvider(AppDbContext dbContext, IMapper mapper)
    {
        _users = dbContext.Set<User>();
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<UserModel> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _users
            .Include(x => x.Devices)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), $"User with id {id} not found");
        }

        return _mapper.Map<UserModel>(user);
    }
}
