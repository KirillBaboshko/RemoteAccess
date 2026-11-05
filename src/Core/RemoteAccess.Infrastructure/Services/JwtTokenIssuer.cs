using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RemoteAccess.Domain.Models;
using RemoteAccess.Domain.Services.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using TokenContext = RemoteAccess.Domain.Services.Token.TokenContext;

namespace RemoteAccess.Infrastructure.Services;

public sealed class JwtTokenIssuer : ITokenIssuer
{
    private readonly JwtOptions _options;

    public JwtTokenIssuer(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<Token> CreateToken(TokenContext context)
    {
        var identity = new ClaimsIdentity(new GenericIdentity(context.Login), new[]
        {
            new Claim("sub", context.Login),
            new Claim("userId", context.UserId.ToString())
        });
        var token = IssueToken(identity);
        return Task.FromResult(token);
    }

    /// <inheritdoc />
    public Task<Token> CreateToken(Guid userId, string login)
    {
        return CreateToken(new TokenContext(userId, login));
    }

    /// <inheritdoc />
    public Task<Token> RefreshToken(string token)
    {
        var decodedJwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var identity = new ClaimsIdentity(new GenericIdentity(decodedJwt.Subject), decodedJwt.Claims);
        return Task.FromResult(IssueToken(identity));
    }

    /// <summary/>
    private Token IssueToken(ClaimsIdentity identity)
    {
        var securityKey = _options.GetSecurityKey();
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateJwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            subject: identity,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.Add(_options.LifeTime));

        return new Token(jwtTokenHandler.WriteToken(token), token.ValidTo);
    }
}
