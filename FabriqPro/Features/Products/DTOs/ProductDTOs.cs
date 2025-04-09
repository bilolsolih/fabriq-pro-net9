namespace FabriqPro.Features.Products.DTOs;

public record ProductDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  
  public required ProductModelDetailDto ProductModel { get; set; }
  public required ProductTypeDetailDto ProductType { get; set; }

  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
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