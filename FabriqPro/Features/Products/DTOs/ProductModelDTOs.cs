namespace FabriqPro.Features.Products.DTOs;

public record ProductModelDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  
  public required ColorDetailDto Color { get; set; }
  public required ProductTypeDetailDto ProductType { get; set; }

  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
}

public record ProductModelListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string Color { get; set; }
  public required string ProductType { get; set; }
}

public record ProductModelCreateDto
{
  public required string Title { get; set; }
  public required int ColorId { get; set; }
  public required int ProductTypeId { get; set; }
}

public record ProductModelUpdateDto
{
  public string? Title { get; set; }
  public int? ColorId { get; set; }
  public int? ProductTypeId { get; set; }
}