namespace Identity.Infrastructure.Entities.Users;

public class User
{
    public Guid Id { get; set; }
    public virtual ApplicationUser Identity { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public Address Address { get; set; }
}