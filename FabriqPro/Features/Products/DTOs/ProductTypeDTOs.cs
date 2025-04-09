namespace FabriqPro.Features.Products.DTOs;

public record ProductTypeDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
}

public record ProductTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record ProductTypeCreateDto
{
  public required string Title { get; set; }
}

public record ProductTypeUpdateDto
{
  public string? Title { get; set; }
}