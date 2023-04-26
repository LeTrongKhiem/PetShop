using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Identity.Infrastructure.Application;

public interface IIdentityService
{
    ClaimsPrincipal User { get; }
}

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new NullReferenceException("IHttpContextAccessor is not accessible in the current context");
    }
    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
}