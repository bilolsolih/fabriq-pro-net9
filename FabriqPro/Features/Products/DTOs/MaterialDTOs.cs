namespace FabriqPro.Features.Products.DTOs;

public record MaterialCreateDto
{
  public required int ProductTypeId { get; set; }
  public required int MaterialTypeId { get; set; }
  public required int Quantity { get; set; }
}

public record MaterialListDto
{
  public required int Id { get; set; }
  public required string MaterialType { get; set; }
  public required string ProductType { get; set; }
}

public record MaterialUpdateDto
{
  public int? MaterialTypeId { get; set; }
  public int? ProductTypeId { get; set; }
}