using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Authentication.Models;

public enum UserRoles
{
    Admin,
    SuperAdmin,
    Master,
    Cutter,
    Packager,
    Sewer,
    Supplier,
    Presser,
    Accountant,
    Cleaner,
    Cook,
}

public class User
{
    public int Id { get; set; }
    
    public required int DepartmentId { get; set; }
    public required Department Department { get; set; }
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePhoto { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Password { get; set; }
    public required string Address { get; set; }
    public required string PassportSeries { get; set; }
    public required DateOnly Birthdate { get; set; }
    public required UserRoles Role { get; set; }

    public double? Salary { get; set; }
    public double? WorkingHours { get; set; }
    public string? WorkingDays { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}