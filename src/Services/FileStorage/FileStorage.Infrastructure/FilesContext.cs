using FileStorage.Infrastructure.Abstractions;
using FileStorage.Infrastructure.Configurations;
using FileStorage.Infrastructure.Encryption;
using Microsoft.EntityFrameworkCore;
using File = FileStorage.Infrastructure.Entities.File;

namespace FileStorage.Infrastructure;

public class FilesContext : DbContext, IUnitOfWork
{
    public DbSet<File> Files { get; set; }
    private readonly IEncrypt _encryptor;

    public FilesContext(DbContextOptions<FilesContext> options, IEncrypt encryptor)
        : base(options)
    {
        _encryptor = encryptor;
    }

    public FilesContext(DbContextOptions<FilesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<File>().HasIndex(x => x.Id);
        modelBuilder.Entity<File>().Property(x => x.FilePath).HasConversion(x => _encryptor.Encrypt(x), x => _encryptor.Decrypt(x));
        modelBuilder.ApplyConfiguration(new FileConfigurations());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync() > 0;
        return result;
    }

    public int SaveEntities()
    {
        var result = base.SaveChanges();
        return result;
    }
}