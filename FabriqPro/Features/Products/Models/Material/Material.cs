using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Material;

public class Material : BaseModel
{
  public required string Title { get; set; }

  public ICollection<MaterialToDepartment> MaterialDepartments { get; set; } = [];
}