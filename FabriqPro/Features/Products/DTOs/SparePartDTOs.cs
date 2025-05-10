using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.DTOs;

public record SparePartCreateDto
{
  public required string Title { get; set; }
}

public record AddSparePartToStorageDto
{
  public required int FromUserId { get; set; }
  public required int SparePartId { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
}

public record GiveSparePartToMasterDto
{
  public required int SparePartToDepartmentId { get; set; }
  public required Department Department { get; set; }
  public required int MasterId { get; set; }
  public required double Quantity { get; set; }
}

public record SparePartTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record SparePartListDto
{
  public required int Id { get; set; }

  public required string FromUser { get; set; }
  public required UserRoles FromUserRole { get; set; }

  public required string ToUser { get; set; }
  public required UserRoles ToUserRole { get; set; }

  public required string AcceptedUser { get; set; }
  public required UserRoles AcceptedUserRole { get; set; }

  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }

  public required DateTime Date { get; set; }
  public required ItemStatus Status { get; set; }
}

public record SparePartFlowListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string PartyNumber { get; set; }

  public required string FromUser { get; set; }
  public required UserRoles FromUserRole { get; set; }

  public required double Width { get; set; }
  public required double Thickness { get; set; }
  public required bool HasPatterns { get; set; }
  public required string Color { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
  public required DateTime Date { get; set; }
  public required ItemStatus Status { get; set; }
}

public record ReturnSparePartDto
{
  public required int ToUserId { get; set; }
  public required double Quantity { get; set; }
  public bool? ReturnAll { get; set; }
  public string? Reason { get; set; }
}