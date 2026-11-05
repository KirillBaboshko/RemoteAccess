using RemoteAccess.Data.DataProviders.Users.Model;
using RemoteAccess.Domain.Services.Token;
namespace RemoteAccess.Application.Features.Users.Models;

public class AuthResult
{
    public Token Token { get; }
    public UserShortModel User { get; }

    public AuthResult(UserShortModel User, Token Token)
    {
        this.User = User;
        this.Token = Token;
    }
}
