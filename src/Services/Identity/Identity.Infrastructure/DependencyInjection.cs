using Identity.Infrastructure.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IdentityContext>();
        services.AddScoped<IIdentityService, IdentityService>();
        return services;
    }
}