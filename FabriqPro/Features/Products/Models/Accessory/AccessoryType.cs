using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Accessory;

public class AccessoryType : BaseModel
{
  public required string Title { get; set; }
  public ICollection<Accessory> AccessoryDepartments { get; set; } = [];
}