namespace FabriqPro.Features.Products.Models;

public class Product
{
    public int Id { get; set; }

    public required int ProductTypeId { get; set; }
    public required ProductType ProductType { get; set; }
    
    public required int ProductModelId { get; set; }
    public required ProductModel ProductModel { get; set; }
    
    public ICollection<Department> Departments { get; set; } = new List<Department>();

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

// product qaysi departmentdan qaysi departmentga o'tyapgani
