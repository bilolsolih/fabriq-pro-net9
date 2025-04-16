using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.ProductParts.Models;

public class ProductPartType
{
     public int Id { get; set; }
    public required string Title { get; set; }
    public required int ProductTypeId { get; set; }
    public required ProductType ProductType { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
