using System.Runtime.Serialization;
using Identity.Infrastructure.Entities.Enum;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Identity.Infrastructure.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public Gender Gender { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public UserType UserType { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum UserType
{
    [EnumMember(Value = "Director")]
    Director = 1,
    [EnumMember(Value = "Manager")]
    Manager = 2,
    [EnumMember(Value = "Employee")]
    Employee = 3,
    [EnumMember(Value = "Customer")]
    Customer = 4,
    [EnumMember(Value = "Guest")]
    Guest = 5
}