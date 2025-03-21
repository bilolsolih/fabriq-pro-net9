namespace FabriqPro.Features.Products.Models;

public class ProductToDepartment
{
    public required int ProductId { get; set; }
    public required int DepartmentId { get; set; }
    public required int Quantity { get; set; }

    public required Department Department { get; set; }
    public required Product Product { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}