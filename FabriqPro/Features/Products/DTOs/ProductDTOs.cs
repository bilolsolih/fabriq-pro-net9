using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.DTOs;

public class AddProductToMasterDto
{
  public required int ProductTypeId { get; set; }
  public required int ProductModelId { get; set; }
  public required int Quantity { get; set; }
}

public class UpdateAddedProductDto
{
  public int? ProductTypeId { get; set; }
  public int? ProductModelId { get; set; }
  public int? Quantity { get; set; }
}

public record ProductsAddedByMeListDto
{
  public required int Id { get; set; }
  public required Department Department { get; set; }
  public required string Master { get; set; }

  public string? FromUser { get; set; }
  public string? ToUser { get; set; }

  public required string ProductModel { get; set; }
  public required string ProductType { get; set; }

  public required int Quantity { get; set; }
  public required ItemStatus Status { get; set; }
}

public record ProductsSentToMeListDto
{
  public required int Id { get; set; }

  public string? FromUser { get; set; }

  public required string ProductModel { get; set; }
  public required string ProductType { get; set; }

  public required int Quantity { get; set; }
  public required ItemStatus Status { get; set; }
}

public record ProductListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string ProductModel { get; set; }
  public required string ProductType { get; set; }
}

public record ProductCreateDto
{
  public required int ProductModelId { get; set; }
  public required int ProductTypeId { get; set; }
  public required int Quantity { get; set; }
}

public record ProductUpdateDto
{
  public int? ProductModelId { get; set; }
  public int? ProductTypeId { get; set; }
}