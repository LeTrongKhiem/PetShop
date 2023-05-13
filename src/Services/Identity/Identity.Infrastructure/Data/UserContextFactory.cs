using Identity.Infrastructure.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var identityService = new IdentityService(new HttpContextAccessor());
        var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        // optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Initial Catalog=PetShop.Identity.API;User ID=sa;Password=nana01218909214;MultipleActiveResultSets=true;Trust Server Certificate=true");
        optionsBuilder.UseSqlServer(connectionString);
        return new UserContext(optionsBuilder.Options, identityService);
    }
}