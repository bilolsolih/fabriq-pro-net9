using FabriqPro.Core;

namespace FabriqPro.Features.Products.Models.Product;

public record ProductToDepartment : BaseModelRecord
{
  public required Department Department { get; set; }

  public required int ProductId { get; set; }
  public required Product Product { get; set; }

  public required int PartyId { get; set; }
  public required Party Party { get; set; }

  public required int Quantity { get; set; }
}