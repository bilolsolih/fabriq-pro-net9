using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Miscellaneous;

public class MiscellaneousType : BaseModel
{
  public required string Title { get; set; }
  public ICollection<Miscellaneous> Miscellaneous { get; set; } = [];
}