using Identity.Application.Abstractions;
using Identity.Infrastructure;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.SeedData;

public class SeedData
{
    public static void EnsureSeedData(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<UserContext>();
            
            context?.Database.Migrate();

            var env = serviceProvider.GetService<IHostEnvironment>();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var contentPath = env?.ContentRootPath;

            var services = scope.ServiceProvider;
            var config = serviceProvider.GetService<IConfiguration>();
            var resource = serviceProvider.GetService<IStringResourceService>();
            
            // var configurationService = new ConfigurationService(config, resource);
            //
            // var defaultPassword = configurationService.GetDefaultPassword();

            if (config != null) InitializeIdentityServer(services, config);

            // var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
            if (context != null && context.Roles.Any())
            {
                if (roleManager != null) SeedRoles(roleManager);
            }
        }
    }

    private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "Director", "Manager", "Employee", "Customer", "Guest" };

        foreach (var role in roles)
        {
            var exists = roleManager.RoleExistsAsync(role);
            
            if (!exists.Result)
            {
                roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    Description = role
                }).Wait();
            }
        }
    }

    private static void InitializeIdentityServer(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        serviceProvider.GetService<PersistedGrantDbContext>()?.Database.Migrate();
        serviceProvider.GetService<ConfigurationDbContext>()?.Database.Migrate();
        
        var context = serviceProvider.GetService<ConfigurationDbContext>();
        var clients = context.Clients.Include(x => x.AllowedScopes).ToList();
        var requestClients = Config.GetClients(configuration.GetSection("Identity:Clients"), 
            configuration.GetSection("Identity:ApiResources"), configuration.GetSection("Identity:IntegrationApis"));

        foreach (var request in requestClients)
        {
            var requestEntity = request.ToEntity();
            if (!clients.Any(x => x.ClientId == request.ClientId))
            {
                context.Clients.Add(requestEntity);
            }
            else
            {
                var client = clients.FirstOrDefault(x => x.ClientId == request.ClientId);
                client.AllowedScopes = requestEntity.AllowedScopes;
                client.AccessTokenType = requestEntity.AccessTokenType;
                context.Clients.Add(client);
            }
        }

        context.SaveChanges();
        
        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }

        var apiResources = context.ApiResources.ToList();
        foreach (var resource in Config.GetApiResources(configuration.GetSection("Identity:IntegrationApis")))
        {
            if (!apiResources.Any(a => a.Name == resource.Name))
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            else
            {
                var res = apiResources.FirstOrDefault(x => x.Name == resource.Name);
                if (res.Secrets == null)
                {
                    res.Secrets = resource.ToEntity().Secrets;
                }
            }
        }
        context.SaveChanges();
    }
}