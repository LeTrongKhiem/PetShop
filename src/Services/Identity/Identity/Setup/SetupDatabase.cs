using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Setup;

public static class SetupDatabase
{
    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddDbContext<UserContext>(opts =>
        {
            opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            opts.EnableSensitiveDataLogging();
        });
        services.AddDbContext<UserContext>();
        services.AddScoped<UserContext>();

        return services;
    }
}