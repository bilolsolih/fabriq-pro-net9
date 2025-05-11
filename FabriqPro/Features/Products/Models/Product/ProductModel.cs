using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Product;

public class ProductModel : BaseModel
{
  public required int ProductTypeId { get; set; }
  public required ProductType ProductType { get; set; }

  public required string Title { get; set; }

  public required int ColorId { get; set; }
  public required Color Color { get; set; }

  public ICollection<Product> Products { get; set; } = [];
  public ICollection<ProductPart.ProductPart> ProductParts { get; set; } = [];
}