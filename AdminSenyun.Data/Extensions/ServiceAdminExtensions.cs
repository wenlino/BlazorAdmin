using AdminSenyun.Data.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace AdminSenyun.Data.Extensions;

public static class ServiceAdminExtensions
{

    public static IServiceCollection AddServiceAdminService(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

        services.AddScoped<IAdmin, AdminService>();

        services.AddScoped<IdentityService>();
        return services;
    }
}
