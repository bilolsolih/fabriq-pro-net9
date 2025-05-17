using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.DTOs;

public record MiscellaneousCreateUpdateDto
{
  public required string Title { get; set; }
}

public record AddMiscellaneousToStorageDto
{
  public required int FromUserId { get; set; }
  public required int MiscellaneousId { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
}

public record GiveMiscellaneousToMasterDto
{
  public required int MiscellaneousToDepartmentId { get; set; }
  public required Department Department { get; set; }
  public required int MasterId { get; set; }
  public required double Quantity { get; set; }
}

public record MiscellaneousTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
}

public record MiscellaneousListAllDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string Quantity { get; set; }
}

public record MiscellaneousListDto
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

public record MiscellaneousFlowListDto
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

public record ReturnMiscellaneousDto
{
  public required int ToUserId { get; set; }
  public required double Quantity { get; set; }
  public bool? ReturnAll { get; set; }
  public string? Reason { get; set; }
}