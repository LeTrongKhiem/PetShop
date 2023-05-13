using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using IdentityServer4;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Identity.Setup;

public static class SetupIdentity
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddDbContext<UserContext>();
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
        });

        // services.AddIdentityServer()
        //     .AddInMemoryCaching()
        //     .AddClientStore<InMemoryClientStore>()
        //     .AddResourceStore<InMemoryResourcesStore>();

        return services;
    }
}