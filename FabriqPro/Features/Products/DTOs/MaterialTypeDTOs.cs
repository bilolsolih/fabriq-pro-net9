namespace FabriqPro.Features.Products.DTOs;

public record MaterialTypeDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }

  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
}

public record MaterialTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record MaterialTypeCreateDto
{
  public required string Title { get; set; }
  public required IEnumerable<int> ProductTypeIds { get; set; }
}

public record MaterialTypeUpdateDto
{
  public string? Title { get; set; }
  public IEnumerable<int>? ProductTypeIds { get; set; }
}