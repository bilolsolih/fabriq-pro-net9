﻿using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Models;

namespace FabriqPro.Features.Products.DTOs;

public record MaterialTypeCreateUpdateDto
{
  public required string Title { get; set; }
}

public record AddMaterialToStorageDto
{
  public required int FromUserId { get; set; }
  public required int MaterialId { get; set; }
  public required int ColorId { get; set; }
  public required double Thickness { get; set; }
  public required double Width { get; set; }
  public required bool HasPatterns { get; set; }
  public required string PartyNumber { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
}

public record GiveMaterialToMasterDto
{
  public required int MaterialId { get; set; }
  public required int CuttingMasterId { get; set; }
  public required double Quantity { get; set; }
}

public record MaterialsListAllDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required string Quantity { get; set; }
}

public record MaterialTypeListDto
{
  public required int Id { get; set; }
  public required string Title { get; set; }
  public required double TotalInKg { get; set; }
  public required double TotalInMeter { get; set; }
  public required double TotalInPack { get; set; }
}

public record MaterialListDto
{
  public required int Id { get; set; }
  public required string PartyNumber { get; set; }

  public required string FromUser { get; set; }
  public required UserRoles FromUserRole { get; set; }
  public required string ToUser { get; set; }
  public required UserRoles ToUserRole { get; set; }
  public required string AcceptedUser { get; set; }
  public required UserRoles AcceptedUserRole { get; set; }

  public required double Width { get; set; }
  public required double Thickness { get; set; }
  public required bool HasPatterns { get; set; }
  public required string Color { get; set; }
  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }
  public required DateTime Date { get; set; }
  public required ItemStatus Status { get; set; }
}

public record MaterialFlowListDto
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

public record ReturnMaterialDto
{
  public required int ToUserId { get; set; }
  public required double Quantity { get; set; }
  public bool? ReturnAll { get; set; } 
  public string? Reason { get; set; }
}

public record MaterialUpdateDto
{
  public int? MaterialTypeId { get; set; }
  public int? ProductTypeId { get; set; }
}

public record CuttingMaterialDto
{
  public required IList<MaterialsUsedInCuttingDto> MaterialsUsed { get; set; }
  public required IList<ProductPartsCutDto> ProductPartsCut { get; set; }
  public required double WasteAmount { get; set; }
}

public record ProductPartsCutDto
{
  public required int ProductPartTypeId { get; set; }
  public required int ProductModelId { get; set; }
  public required int Quantity { get; set; }
}

public record MaterialsUsedInCuttingDto
{
  public required int MaterialId { get; set; }
  public required double Quantity { get; set; }
  public required bool UsedAll { get; set; }
}