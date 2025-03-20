namespace FabriqPro.Features.Clients.Controllers.Filters;

public enum OrderBy
{
    Id,
    FullName,
    PhoneNumber,
    PurchasesCount,
    Address,
}

public class ClientFilters
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? Search { get; set; }
    public OrderBy? Order { get; set; }
    public bool? Descending { get; set; }
}