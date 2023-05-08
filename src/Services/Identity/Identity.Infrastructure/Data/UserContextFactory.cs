using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure.Data;

public class UserContextFactory: IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
        optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Initial Catalog=PetShop.Identity.API;User ID=sa;Password=nana01218909214;MultipleActiveResultSets=true;Trust Server Certificate=true");

        return new UserContext(optionsBuilder.Options);
    }
}