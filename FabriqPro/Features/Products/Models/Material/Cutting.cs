using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Material;

public class Cutting : BaseModel
{
  public required int MasterId { get; set; }
  public User Master { get; set; }
  public required ICollection<Material> MaterialsUsed { get; set; }
  public required ICollection<ProductPart.ProductPart> ProducedParts { get; set; }
  public required double Waste { get; set; }
}