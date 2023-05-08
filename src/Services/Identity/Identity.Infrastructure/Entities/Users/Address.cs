namespace Identity.Infrastructure.Entities.Users;

public class Address
{
    public int Id { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string Line3 { get; set; }
    public string Line4 { get; set; }
    public string Region { get; set; }
    public Guid? UserId { get; set; }
}