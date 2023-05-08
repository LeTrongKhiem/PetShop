using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Setup;

public static class SetupIdentity
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}