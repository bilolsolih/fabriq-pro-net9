using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Miscellaneous;

public class Miscellaneous : BaseModel
{
  public required string Title { get; set; }
  public ICollection<MiscellaneousDepartment> MiscellaneousDepartments { get; set; } = [];
}