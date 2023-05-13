using Identity.Infrastructure.Entities;
using IdentityServer4.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Setup;

public static class SetupIdentityServer
{
    public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, string issuerUri, string connectionString, string migrationsAssembly)
    {
        var builder = services.AddIdentityServer(opts =>
            {
                opts.Events.RaiseErrorEvents = true;
                opts.Events.RaiseInformationEvents = true;
                opts.Events.RaiseFailureEvents = true;
                opts.Events.RaiseSuccessEvents = true;

                if (!string.IsNullOrWhiteSpace(issuerUri))
                {
                    opts.IssuerUri = issuerUri;
                }
            })
            // this adds the config data from DB (clients, resources)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(connectionString,
                        optionsBuilder =>
                            optionsBuilder.MigrationsAssembly(migrationsAssembly));
                };
            })
            // this adds the operational data from DB (codes, tokens, consents)
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(connectionString,
                        optionsBuilder =>
                            optionsBuilder.MigrationsAssembly(migrationsAssembly));
                };

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<ApplicationUser>();
        return builder;
    }
}