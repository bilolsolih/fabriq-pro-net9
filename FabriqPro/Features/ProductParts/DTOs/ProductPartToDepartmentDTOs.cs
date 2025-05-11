using FabriqPro.Features.Products.Models;
using FabriqPro.Features.ProductParts.Models;
using ProductPart = FabriqPro.Features.ProductParts.Models.ProductPart;

namespace FabriqPro.Features.ProductParts.DTOs;

public record ProductPartToDepartmentCreateDto
{
    public required int ProductPartId { get; set; }
    public required int DepartmentId { get; set; }
    public required int Quantity { get; set; }
}

public record ProductPartToDepartmentDetailDto
{
    public required int Id { get; set; }
    public required int ProductPartId { get; set; }
    public required ProductPart ProductPart { get; set; }
    public required int DepartmentId { get; set; }
    public required Department Department { get; set; }
    public required int Quantity { get; set; }
    public required DateTime Created { get; set; }
    public required DateTime Updated { get; set; }
}

public record ProductPartToDepartmentListDto
{
    public required int Id { get; set; }
    public required int ProductPartId { get; set; }
    public required string ProductPartName { get; set; }
    public required int DepartmentId { get; set; }
    public required string DepartmentName { get; set; }
    public required int Quantity { get; set; }
}

public record ProductPartToDepartmentUpdateDto
{
    public int? ProductPartId { get; set; }
    public int? DepartmentId { get; set; }
    public int? Quantity { get; set; }
}