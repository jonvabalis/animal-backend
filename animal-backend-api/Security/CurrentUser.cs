using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace animal_backend_api.Security;

public static class CurrentUser
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        // Try common locations for user id
        var raw =
            user.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
            user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user.FindFirstValue("sub");

        if (raw is null || !Guid.TryParse(raw, out var userId))
            throw new UnauthorizedAccessException("Invalid token (missing user id).");

        return userId;
    }
}