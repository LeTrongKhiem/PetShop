using Identity.Infrastructure.Application;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public readonly IIdentityService _identityService;

    public const string DEFAULT_SCHEMA = "user";
    public const string ACCESS_SCHEMA = "access";
    
    public IdentityContext(DbContextOptions<IdentityContext> options, IIdentityService identityService) : base(options)
    {
        _identityService = identityService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().HasMany(x => x.Roles).WithOne().HasForeignKey(u => u.UserId).IsRequired();
    }
}