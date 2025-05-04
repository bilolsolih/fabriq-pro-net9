using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.ProductParts.Models;

public class ProductPart
{
  public int Id { get; set; }
  public required int ProductTypeId { get; set; }
  public required ProductType ProductType { get; set; }
  public required int ProductPartTypeId { get; set; }
  public required ProductPartType ProductPartType { get; set; }
  public required int ProductModelId { get; set; }
  public required ProductModel ProductModel { get; set; }
  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}