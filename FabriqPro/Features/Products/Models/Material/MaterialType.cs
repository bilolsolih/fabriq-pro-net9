using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.Products.Models;

public class MaterialType
{
  public int Id { get; set; }
  public required string Title { get; set; }

  public required ICollection<ProductType> ProductTypes { get; set; } = [];

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}