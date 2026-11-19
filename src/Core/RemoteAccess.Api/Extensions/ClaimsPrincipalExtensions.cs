using System.Security.Claims;
using RemoteAccess.Domain.Models.Ecxeptions;

namespace RemoteAccess.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) =>
        Guid.TryParse(user.FindFirst("userId")?.Value, out var id)
            ? id
            : throw new InvalidOperationException(ValidationErrorMessages.UserIdMissingFromToken);
}
