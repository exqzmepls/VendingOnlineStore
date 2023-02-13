using System.Security.Claims;
using Core.Identity;

namespace VendingOnlineStore.Identity;

public class HttpContextUserIdentityProvider : IUserIdentityProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserIdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserIdentifier()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == default)
            throw new InvalidOperationException("No active HttpContext");

        var user = httpContext.User;
        var nameIdentifierClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (nameIdentifierClaim == default)
            throw new InvalidOperationException($"No {nameof(ClaimTypes.NameIdentifier)} found");

        var identifier = Guid.Parse(nameIdentifierClaim.Value);
        return identifier;
    }
}