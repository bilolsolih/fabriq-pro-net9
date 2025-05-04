using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.ProductParts.DTOs;

public record ProductPartTypeCreateDto
{
    public required string Title { get; set; }
    public required int ProductTypeId { get; set; }
}

public record ProductPartTypeDetailDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required int ProductTypeId { get; set; }
    public required ProductType ProductType { get; set; }
}

public record ProductPartTypeListDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required int ProductTypeId { get; set; }
    public required string ProductTypeName { get; set; }
}

public record ProductPartTypeUpdateDto
{
    public string? Title { get; set; }
    public int? ProductTypeId { get; set; }
}