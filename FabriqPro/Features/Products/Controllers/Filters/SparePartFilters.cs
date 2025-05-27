using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.Controllers.Filters;

public class SparePartFilters
{
  public int? TypeId { get; set; }
  public int? FromUserId { get; set; }
  public int? ToUserId { get; set; }
  public ItemStatus? Status { get; set; }
  public int? Limit { get; set; }
  public int? Page { get; set; }
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
}