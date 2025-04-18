using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.ProductParts.Models;

public class ProductPartToDepartment
{
    public int Id { get; set; }
    public required int ProductPartId { get; set; }
    public required ProductPart ProductPart { get; set; }
    public required int DepartmentId { get; set; }
    public required Department Department { get; set; }
    public required int Quantity { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
