using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.DTOs;

public record MaterialCreateDto
{
  public required string Title { get; set; }
}

public record AddMaterialToStorageDto
{
  public required int MaterialId { get; set; }
  public required string PartyNumber { get; set; }
  public required double Quantity { get; set; }
}

public record MaterialTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record MaterialListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string PartyNumber { get; set; }
  public required string Employee { get; set; }
  public required string EmployeeRole { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
  public required DateOnly Date { get; set; }
}

public record MaterialUpdateDto
{
  public int? MaterialTypeId { get; set; }
  public int? ProductTypeId { get; set; }
}