using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RemoteAccess.Infrastructure.Services;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public SecurityKey? SecurityKey { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Срок жизни токена
    /// По-умолчанию 30 дней
    /// </summary>
    public TimeSpan LifeTime { get; set; } = TimeSpan.FromDays(1);

    /// <summary>
    /// Конструктор без параметров для биндинга конфигурации
    /// </summary>
    public JwtOptions()
    {
    }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    public JwtOptions(string issuer, string secret)
    {
        Issuer = issuer;
        Secret = secret;
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{" + secret + "}"));
    }

    /// <summary>
    /// Получить SecurityKey (создается из Secret если не задан явно)
    /// </summary>
    public SecurityKey GetSecurityKey()
    {
        if (SecurityKey != null)
            return SecurityKey;

        if (string.IsNullOrEmpty(Secret))
            throw new InvalidOperationException("Secret is not configured");

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{" + Secret + "}"));
    }

    /// <summary>
    /// Конфигурация по умолчанию
    /// </summary>
    public static JwtOptions Default => new("RemoteAccess.Default", "PlnXr2(&E6#0ja&5N5r0JACV&%E1(c%2x26#OQ9&d3%)16Wky544otB&)79&*N6HX*8%GJ4Ed5*Fw4c&%*kJ0&p3#j75ROtC0wzo#4I*(1z(6sU5");
}
