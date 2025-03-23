namespace FabriqPro.Features.Products.DTOs;

public record ColorCreateDto
{
  public required string Title { get; set; }
  public required string ColorCode { get; set; }
}

public record ColorDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string ColorCode { get; set; }
  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
}

public class ColorListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string ColorCode { get; set; }
}

public class ColorUpdateDto
{
  public string? Title { get; set; }
  public string? ColorCode { get; set; }
}