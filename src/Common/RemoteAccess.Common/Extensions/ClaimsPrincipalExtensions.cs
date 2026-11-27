using System.Security.Claims;
using RemoteAccess.Common.Security;

namespace RemoteAccess.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public const string UserIdClaimType = "userId";

    public static Guid GetUserId(this ClaimsPrincipal user) =>
        TryParseUserId(user, out var userId)
            ? userId
            : throw new InvalidOperationException(ValidationErrorMessages.UserIdMissingFromToken);

    public static bool TryGetNonEmptyUserId(this ClaimsPrincipal? user, out Guid userId) =>
        TryParseUserId(user, out userId) && userId != Guid.Empty;

    private static bool TryParseUserId(ClaimsPrincipal? user, out Guid userId) =>
        Guid.TryParse(user?.FindFirst(UserIdClaimType)?.Value, out userId);
}
