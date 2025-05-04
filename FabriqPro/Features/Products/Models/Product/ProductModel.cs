namespace FabriqPro.Features.Products.Models.Product;

public class ProductModel
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required int ColorId { get; set; }
    public required Color Color { get; set; }
    
    public required int ProductTypeId { get; set; }
    public required ProductType ProductType { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}