namespace FabriqPro.Features.Products.Models;

public enum Department
{
  Storage,
  Cutting,
  Sewing,
  Cleaning,
  Pressing,
  Packaging
}


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
  AddedByMaster,
}