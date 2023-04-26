using Identity.Application.Abstractions;
using Identity.Infrastructure;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.SeedData;

public class SeedData
{
    public static void EnsureSeedData(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<IdentityContext>();
            
            context.Database.Migrate();

            var env = serviceProvider.GetService<IHostEnvironment>();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var contentPath = env.ContentRootPath;
            var resource = serviceProvider.GetService<IStringResourceService>();
            
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
}