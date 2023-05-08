using Identity.Infrastructure.Application;
using Identity.Infrastructure.Data.Configurations;
using Identity.Infrastructure.Entities;
using Identity.Infrastructure.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

public class UserContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly IIdentityService _identityService;
    public DbSet<Address> Addresses { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options, IIdentityService identityService) : base(options)
    {
        _identityService = identityService;
    }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().HasMany(x => x.Roles).WithOne().HasForeignKey(u => u.UserId).IsRequired();

        modelBuilder.ApplyConfiguration(new AddressEntityConfiguration());
    }
}