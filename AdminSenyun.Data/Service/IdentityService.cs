using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace AdminSenyun.Data.Service;

public class IdentityService(IHttpContextAccessor httpContextAccessor)
{
    public IIdentity? GetUserIdentity()
    {
        return httpContextAccessor.HttpContext?.User?.Identity;
    }

    public string? UserName => GetUserName();
    public string? GetUserName()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
