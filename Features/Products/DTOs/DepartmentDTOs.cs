namespace FabriqPro.Features.Products.DTOs;

public record DepartmentDetailDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required DateTime Created { get; set; }
  public required DateTime Updated { get; set; }
}

public record DepartmentListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record DepartmentCreateDto
{
  public required string Title { get; set; }
}

public record DepartmentUpdateDto
{
  public string? Title { get; set; }
}