using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Material;

public class Material : BaseModel
{
  public required string Title { get; set; }
  // public required int ProductTypeId { get; set; }
  // public required ProductType ProductType { get; set; }
}