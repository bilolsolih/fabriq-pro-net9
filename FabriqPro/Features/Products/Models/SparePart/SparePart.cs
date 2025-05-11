using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.SparePart;

public record SparePart : BaseModelRecord
{
  public required Department Department { get; set; }

  public int? OriginId { get; set; }
  public SparePart Origin { get; set; }

  public ICollection<SparePart> Transfers { get; set; } = [];

  public required int SparePartId { get; set; }
  public SparePartType SparePartType { get; set; }

  public required int AcceptedUserId { get; set; }
  public User AcceptedUser { get; set; }

  public required int FromUserId { get; set; }
  public User FromUser { get; set; }

  public required int ToUserId { get; set; }
  public User ToUser { get; set; }
  
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }

  public required ItemStatus Status { get; set; }
}