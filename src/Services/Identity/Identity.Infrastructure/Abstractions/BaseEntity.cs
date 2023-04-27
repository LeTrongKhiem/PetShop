using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure.Abstractions;

public abstract class BaseEntity<T>
{
    [Key] public T Id { get; set; }
}

public interface IAuditableEntity
{
    public DateTime DateCreated { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
}

public interface IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public Guid? DeletedBy { get; set; }
}