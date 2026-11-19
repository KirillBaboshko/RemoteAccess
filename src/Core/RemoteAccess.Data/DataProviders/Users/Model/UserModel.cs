using RemoteAccess.Domain.Models.Users;

namespace RemoteAccess.Data.DataProviders.Users.Model
{
    /// <summary>
    /// Выходная модель пользователя
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public UserEmail Email { get; init; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime CreatedAt { get; init; }

    }
}
