using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            // var resource = serviceProvider.GetService<IStringResourceService>();

            // var configurationService = new ConfigurationService(config, resource);
            //
            // var defaultPassword = configurationService.GetDefaultPassword();

            if (config != null) InitializeIdentityServer(services, config);

            // var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            if (context != null && !context.Roles.Any())
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
        serviceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        serviceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

        var context = serviceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        var clients = context.Clients.Include(x => x.AllowedScopes).ToList();
        var requestClients = Config.GetClients(configuration.GetSection("Identity:Clients"),
            configuration.GetSection("Identity:ApiResources"), configuration.GetSection("Identity:IntegrationApis"));

        foreach (var rc in requestClients)
        {
            var rcEntity = rc.ToEntity();
            if (!clients.Any(c => c.ClientId == rc.ClientId))
            {
                context.Clients.Add(rcEntity);
            }
            else
            {
                var client = clients.FirstOrDefault(c => c.ClientId == rc.ClientId);
                client.AllowedScopes = rcEntity.AllowedScopes;
                client.AccessTokenType = rcEntity.AccessTokenType;
                context.Clients.Update(client);
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

    private static void SeedDataFromJson<T>(UserContext context, string filePath) where T : class
    {
        if (!context.Set<T>().Any())
        {
            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath);
                var entities = JsonConvert.DeserializeObject<List<T>>(content);

                context.Set<T>().AddRange(entities);
                context.SaveChanges();
            }
        }
    }
}