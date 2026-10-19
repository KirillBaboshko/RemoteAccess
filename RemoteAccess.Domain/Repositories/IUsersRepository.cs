using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    Task CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение пользователя по email
    /// </summary>
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверка существования пользователя по email
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}
