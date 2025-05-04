using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.SparePart;

public class SparePartDepartment
{
  public required Department Department { get; set; }
  public required int UserId { get; set; }
  public User User { get; set; }
  
  public required int SparePartId { get; set; }
  public SparePart SparePart { get; set; }
  
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
}