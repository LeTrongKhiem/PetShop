using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = FileStorage.Infrastructure.Entities.File;

namespace FileStorage.Infrastructure.Configurations;

public class FileConfigurations : IEntityTypeConfiguration<File>
{
    public FileConfigurations()
    {
        
    }

    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FilePath).IsRequired();

        builder.Property(o => o.ThumbnailPath).IsRequired();

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}