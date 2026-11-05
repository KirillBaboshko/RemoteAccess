namespace RemoteAccess.Domain.Services.Token;

/// <summary>
/// Контекст владельца токена: кто аутентифицирован.
/// </summary>
public sealed class TokenContext
{
    public Guid UserId { get; }

    public string Login { get; }

    public TokenContext(Guid userId, string login)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId не задан", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login не задан", nameof(login));
        }

        UserId = userId;
        Login = login.ToLowerInvariant();
    }
}
