using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Material;

public class MaterialType : BaseModel
{
  public required string Title { get; set; }

  public ICollection<Material> MaterialDepartments { get; set; } = [];
}