﻿using FabriqPro.Core;
using FabriqPro.Features.Authentication.Models;

namespace FabriqPro.Features.Products.Models.Accessory;

public record Accessory : BaseModelRecord
{
  public required Department Department { get; set; }

  public int? OriginId { get; set; }
  public Accessory Origin { get; set; }

  public ICollection<Accessory> Transfers { get; set; } = [];

  public required int AcceptedUserId { get; set; }
  public User AcceptedUser { get; set; }

  public required int FromUserId { get; set; }
  public User FromUser { get; set; }

  public required int ToUserId { get; set; }
  public User ToUser { get; set; }

  public required int AccessoryTypeId { get; set; }
  public AccessoryType AccessoryType { get; set; }

  public required double Quantity { get; set; }
  public required Unit Unit { get; set; }

  public required ItemStatus Status { get; set; }
}