using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Material;

public record MaterialToDepartment : BaseModelRecord
{
  public int? OriginId { get; set; }
  public MaterialToDepartment Origin { get; set; }
  
  public ICollection<MaterialToDepartment> Transfers { get; set; } = [];
  
  public required Department Department { get; set; }

  public required int MaterialId { get; set; }
  public Material Material { get; set; }

  public required int AcceptedUserId { get; set; }
  public User AcceptedUser { get; set; }

  public required int FromUserId { get; set; }
  public User FromUser { get; set; }

  public required int ToUserId { get; set; }
  public User ToUser { get; set; }

  public required int PartyId { get; set; }
  public Party Party { get; set; }

  public required int ColorId { get; set; }
  public Color Color { get; set; }

  public required double Thickness { get; set; }
  public required double Width { get; set; }
  public required bool HasPatterns { get; set; }

  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }

  public required ItemStatus Status { get; set; }
}