using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.SparePart;

public class SparePart : BaseModel
{
  public required string Title { get; set; }
  public ICollection<SparePartDepartment> SparePartDepartments { get; set; } = [];
}