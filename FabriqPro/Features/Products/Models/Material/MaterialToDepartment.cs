using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Material;

public class MaterialToDepartment
{
  public required Department Department { get; set; }

  public required int MaterialId { get; set; }
  public Material Material { get; set; }

  public required int UserId { get; set; }
  public User User { get; set; }

  public required int PartyId { get; set; }
  public Party Party { get; set; }

  public required int ColorId { get; set; }
  public Color Color { get; set; }

  public required double Thickness { get; set; }
  public required double Width { get; set; }
  public required bool HasPatterns { get; set; }

  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }

  public DateTime Created { get; set; }
  public DateTime Updated { get; set; }
}