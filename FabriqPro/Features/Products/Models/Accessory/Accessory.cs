using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Accessory;

public class Accessory : BaseModel
{
  public required string Title { get; set; }
  public ICollection<AccessoryDepartment> AccessoryDepartments { get; set; } = [];
}