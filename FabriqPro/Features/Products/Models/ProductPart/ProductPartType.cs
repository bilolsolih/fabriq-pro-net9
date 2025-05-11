using FabriqPro.Core;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.Products.Models.ProductPart;

public class ProductPartType : BaseModel
{
  public required int ProductTypeId { get; set; }
  public ProductType ProductType { get; set; }

  public required string Title { get; set; }

  public ICollection<ProductPart> ProductParts { get; set; } = [];
}