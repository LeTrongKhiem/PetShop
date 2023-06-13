using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Infrastructure.Abstractions;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    protected EntityTypeBuilder<T> Builder { get; private set; }
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        this.Builder = builder;
        Configure();
    }
    public abstract void Configure();
}