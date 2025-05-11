using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.SparePart;

public class SparePartType : BaseModel
{
  public required string Title { get; set; }
  public ICollection<SparePart> SpareParts { get; set; } = [];
}