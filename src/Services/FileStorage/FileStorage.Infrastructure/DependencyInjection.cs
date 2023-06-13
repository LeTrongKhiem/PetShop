using FileStorage.Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FilesContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddDbContext<KeysContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //services.AddDbContext<FilesContext>();
        //services.AddDbContext<KeysContext>();
        services.AddScoped<KeysContext>();
        services.AddScoped<FilesContext>();
        return services;
    }
}