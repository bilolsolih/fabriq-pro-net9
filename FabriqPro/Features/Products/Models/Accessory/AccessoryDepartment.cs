using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Accessory;

public class AccessoryDepartment
{
  public required Department Department { get; set; }
  public required int UserId { get; set; }
  public User User { get; set; }

  public required int AccessoryId { get; set; }
  public Accessory Accessory { get; set; }

  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
}