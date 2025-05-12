using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models.Product;

namespace FabriqPro.Features.Products.Models.ProductPart;

public record ProductPart : BaseModelRecord
{
  public int? OriginId { get; set; }
  public ProductPart Origin { get; set; }

  public ICollection<ProductPart> Transfers { get; set; } = [];
  
  public required Department Department { get; set; }
  
  public required int MasterId { get; set; }
  public User Master { get; set; }

  public int? FromUserId { get; set; }
  public User FromUser { get; set; }

  public int? ToUserId { get; set; }
  public User ToUser { get; set; }

  public required int ProductPartTypeId { get; set; }
  public ProductPartType ProductPartType { get; set; }
  
  public required int ProductModelId { get; set; }
  public ProductModel ProductModel { get; set; }

  public required double Quantity { get; set; }

  public required ItemStatus Status { get; set; }
}