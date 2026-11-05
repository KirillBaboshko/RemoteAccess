namespace RemoteAccess.Domain.Services.Token;

/// <summary>
/// Выданный пользователю токен доступа.
/// </summary>
public class Token
{
    public string Value { get; }

    public DateTime ExpiresAt { get; }

    public Token( string value, DateTime expiresAt)
    {
        Value = value;
        ExpiresAt = expiresAt;
    }
}
