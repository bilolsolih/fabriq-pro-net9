using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.ProductParts.DTOs;

public record ProductPartCreateDto
{
    public required int ProductTypeId { get; set; }
    public required int ProductPartTypeId { get; set; }
    public required int ProductModelId { get; set; }
}

public record ProductPartDetailDto
{
    public required int Id { get; set; }
    public required int ProductTypeId { get; set; }
    public required ProductType ProductType { get; set; }
    public required int ProductPartTypeId { get; set; }
    public required ProductPartType ProductPartType { get; set; }
    public required int ProductModelId { get; set; }
    public required ProductModel ProductModel { get; set; }
}

public record ProductPartListDto
{
    public required int Id { get; set; }
    public required int ProductTypeId { get; set; }
    public required string ProductTypeName { get; set; }
    public required int ProductPartTypeId { get; set; }
    public required string ProductPartTypeName { get; set; }
    public required int ProductModelId { get; set; }
    public required string ProductModelName { get; set; }
}

public record ProductPartUpdateDto
{
    public int? ProductTypeId { get; set; }
    public int? ProductPartTypeId { get; set; }
    public int? ProductModelId { get; set; }
}