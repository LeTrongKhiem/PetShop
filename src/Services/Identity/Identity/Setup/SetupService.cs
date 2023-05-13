using Identity.Application.Abstractions;
using Identity.Application.Service;
using Identity.Infrastructure.Application;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Identity.Setup;

public static class SetupService
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStringResourceService, StringResourceService>();
        services.AddSingleton<IIdentityService, IdentityService>();
        return services;
    }
}