namespace FabriqPro.Features.Products.Models;

public enum Department
{
  Storage,
  Cutting,
  Sewing,
  Cleaning,
  Pressing,
  Packaging,
  Suppliers
}


public enum Unit
{
  Kg,
  Meter,
  Piece,
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