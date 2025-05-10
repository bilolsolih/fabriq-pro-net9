namespace FabriqPro.Features.Products.Models;

public enum Unit
{
  Kg,
  Meter,
  Pack
}

public enum ItemStatus
{
  Pending,
  Accepted,
  Rejected,
  AcceptedToStorage,
  ReturnedToStorage,
  ReturnedToSupplier,
}