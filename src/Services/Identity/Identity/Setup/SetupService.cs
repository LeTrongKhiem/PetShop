using Identity.Application.Abstractions;
using Identity.Application.Service;

namespace Identity.Setup;

public static class SetupService
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStringResourceService, StringResourceService>();

        return services;
    }
}