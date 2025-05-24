using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Material;

public record Material : BaseModelRecord
{
  public int? OriginId { get; set; }
  public Material Origin { get; set; }
  
  public ICollection<Material> Transfers { get; set; } = [];
  
  public required Department Department { get; set; }

  public required int MaterialTypeId { get; set; }
  public MaterialType MaterialType { get; set; }

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
  
  public required DateTime Date { get; set; }
}