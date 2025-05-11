using FabriqPro.Core;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;

namespace FabriqPro.Features.Authentication.Models;

public enum UserRoles
{
  SuperAdmin,
  Admin,
  CuttingMaster,
  SewingMaster,
  PackagingMaster,
  StorageManager,
  Cutter,
  Packager,
  Sewer,
  Supplier,
  Presser,
  Accountant,
  Cleaner,
  Cook,
}

public class User : BaseModel
{
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

  public ICollection<MaterialToDepartment> AcceptedMaterials { get; set; } = [];
  public ICollection<MaterialToDepartment> SentMaterials { get; set; } = [];
  public ICollection<MaterialToDepartment> ReceivedMaterials { get; set; } = [];
}