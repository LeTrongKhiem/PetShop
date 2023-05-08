using Identity.Infrastructure.Application;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<UserContext>();
        services.AddScoped<IIdentityService, IdentityService>();
        return services;
    }
}