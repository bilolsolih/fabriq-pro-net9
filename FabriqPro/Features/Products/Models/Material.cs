namespace FabriqPro.Features.Products.Models;

public class Material
{
  public int Id { get; set; }
  
  public required int ProductTypeId { get; set; }
  public required ProductType ProductType { get; set; }

  public required int MaterialTypeId { get; set; }
  public required MaterialType MaterialType { get; set; }

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}