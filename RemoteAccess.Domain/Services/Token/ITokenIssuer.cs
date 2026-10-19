using RemoteAccess.Domain.Models;

namespace RemoteAccess.Domain.Services.Token;

public interface ITokenIssuer
{
    /// <summary>
    /// Создание токена
    /// </summary>
    /// <param name="context"><see cref="TokenContext"/></param>
    /// <returns><see cref="Token"/></returns>
    Task<Token> CreateToken(TokenContext context);

    /// <summary>
    /// Создание токена
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="login">Логин пользователя</param>
    /// <returns><see cref="Token"/></returns>
    Task<Token> CreateToken(Guid userId, string login);

    /// <summary>
    /// Обновление токена
    /// </summary>
    /// <param name="token">Обновляемый токен</param>
    /// <returns><see cref="Token"/></returns>
    Task<Token> RefreshToken(string token);
}
