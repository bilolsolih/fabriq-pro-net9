using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Accessory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/accessories"), Authorize]
public class AccessoryController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  [HttpPost("create-new-accessory-type"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<AccessoryCreateDto>> CreateAccessory(AccessoryCreateDto payload)
  {
    var alreadyExists = await context.AccessoryTypes.AnyAsync(sp => sp.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newAccessory = new Accessory { Title = payload.Title };
    context.AccessoryTypes.Add(newAccessory);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpPost("accept-accessory-to-storage"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<AddAccessoryToStorageDto>> AddToStorage(AddAccessoryToStorageDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var fromUser = await context.Users.FindAsync(payload.FromUserId);
    DoesNotExistException.ThrowIfNull(fromUser, $"fromUserId: {payload.FromUserId}");

    if (fromUser.Role != UserRoles.Supplier)
    {
      return Forbid("Aksessuar faqat yetkazib beruvchidan qabul qilib olinishi mumkin.");
    }

    var accessory = await context.AccessoryTypes.FindAsync(payload.AccessoryId);
    DoesNotExistException.ThrowIfNull(accessory, "Omborga mavjud bo'lmagan turdagi 'Aksessuar' qo'shilmoqda.");

    var accessoryDepartment = new AccessoryDepartment
    {
      Department = Department.Storage,
      FromUserId = payload.FromUserId,
      AcceptedUserId = user.Id,
      ToUserId = user.Id,
      AccessoryId = accessory.Id,
      Quantity = payload.Quantity,
      Unit = payload.Unit,
      Status = ItemStatus.AcceptedToStorage,
    };

    context.Accessories.Add(accessoryDepartment);

    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("list-all-accessory-types")]
  public async Task<ActionResult<List<AccessoryTypeListDto>>> ListAllAccessoryTypes()
  {
    var allAccessories = await context.AccessoryTypes
      .ProjectTo<AccessoryTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
  }

  [HttpGet("list-all-accessories")]
  public async Task<ActionResult<List<AccessoryListDto>>> ListAllAccessories()
  {
    var allAccessories = await context.Accessories
      .Include(sp => sp.AcceptedUser)
      .Include(sp => sp.FromUser)
      .Include(sp => sp.Accessory)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage)
      .ProjectTo<AccessoryListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
  }

  [HttpPost("give-to-master"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<GiveAccessoryToMasterDto>> TransferToMaster(GiveAccessoryToMasterDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var fromUser = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(fromUser, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var toUser = await context.Users.FindAsync(payload.MasterId);
    DoesNotExistException.ThrowIfNull(toUser, $"toUserId: {payload.MasterId}");

    if (toUser is not { Role: UserRoles.CuttingMaster or UserRoles.PackagingMaster or UserRoles.SewingMaster })
    {
      return BadRequest("Faqat Masterga o'tkazib berish mumkin.");
    }

    var source = await context.Accessories.FindAsync(payload.AccessoryToDepartmentId);
    DoesNotExistException.ThrowIfNull(source, $"accessoryDepartmentId: {payload.AccessoryToDepartmentId}");

    if (payload.Quantity > source.Quantity)
    {
      return BadRequest("O'tkazilayapgan Aksessuar miqdori mavjud miqdordan ko'p.");
    }

    var newAccessoryToDepartment = source with
    {
      Department = payload.Department,
      FromUserId = fromUser.Id,
      ToUserId = toUser.Id,
      Quantity = payload.Quantity,
      Status = ItemStatus.Pending,
    };

    source.Quantity -= payload.Quantity;

    context.Accessories.Add(newAccessoryToDepartment);
    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("accessories-sent-to-me"), Authorize(Policy = "Master")]
  public async Task<ActionResult<IEnumerable<AccessoryFlowListDto>>> GetAccessoriesSentToMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var result = await context.Accessories
      .Where(m => m.ToUserId == user.Id)
      .ProjectTo<AccessoryFlowListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(result);
  }
  
  [HttpGet("accept-or-reject-sent-accessories/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> AcceptOrRejectSentAccessories(int id, [FromBody] bool accept)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var accessory = await context.Accessories.SingleOrDefaultAsync(sp => sp.Id == id);
    DoesNotExistException.ThrowIfNull(accessory, $"accessoryInDepartmentId: {id}");

    if (accessory.Status != ItemStatus.Pending)
    {
      return Forbid("Bu aksessuar bilan ortiq bu amalni bajarib bo'lmaydi. U qabul/rad qilib bo'lingan.");
    }

    if (accessory.ToUserId != user.Id)
    {
      return Forbid("Bu aksessuar sizga jo'natilmagan.");
    }

    if (accept)
    {
      accessory.Status = ItemStatus.Accepted;
    }
    else if (!accept)
    {
      accessory.Status = ItemStatus.Rejected;
      var originalAccessory = await context.Accessories.FindAsync(accessory.OriginId);
      DoesNotExistException.ThrowIfNull(originalAccessory, "Aksessuar rad etildi, lekin ombordagi ildizi topilmadi.");

      originalAccessory.Quantity += accessory.Quantity;
    }

    await context.SaveChangesAsync();
    return Ok();
  }

  [HttpPost("return-accessory-to-storage/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> ReturnAccessoryToStorage(int id, ReturnAccessoryDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var toUser = await context.Users.FindAsync(payload.ToUserId);
    DoesNotExistException.ThrowIfNull(toUser, "Mavjud bo'lmagan Xodimga aksessuar qaytarilyapti.");

    if (toUser.Role != UserRoles.StorageManager)
    {
      return Forbid("Aksessuar faqat Omborxona menejerlariga qaytarilishi mumkin.");
    }

    var accessory = await context.Accessories.SingleOrDefaultAsync(m => m.Id == id);
    DoesNotExistException.ThrowIfNull(accessory, $"materialInDepartmentId: {id}");

    if (accessory.Status != ItemStatus.Accepted)
    {
      return Forbid("Aksessuar 'Qabul qilingan' statusida emas.");
    }

    if (user.Role != UserRoles.SuperAdmin || accessory.ToUserId != user.Id)
    {
      return Forbid("Bu amalni bajarish uchun yoki SuperAdmin yoki aksessuar qabul qilgan Master bo'lishingiz kerak.");
    }

    if (payload.Quantity > accessory.Quantity)
    {
      return BadRequest("Qaytarilyapgan aksessuar miqdori Masterda mavjud miqdordan ko'p. Amal imkonsiz.");
    }

    var originAccessory = await context.Accessories.FindAsync(accessory.OriginId);
    DoesNotExistException.ThrowIfNull(
      originAccessory, "Aksessuar qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi."
    );

    var newAccessoryTransfer = accessory with
    {
      FromUserId = user.Id,
      ToUserId = toUser.Id,
      Quantity = payload.ReturnAll == true ? accessory.Quantity : payload.Quantity,
      Department = Department.Storage,
      Status = ItemStatus.ReturnedToStorage,
    };

    accessory.Quantity = payload.ReturnAll == true ? 0 : accessory.Quantity - payload.Quantity;
    originAccessory.Quantity += payload.ReturnAll == true ? accessory.Quantity : payload.Quantity;
    context.Accessories.Add(newAccessoryTransfer);
    await context.SaveChangesAsync();
    return Ok();
  }
}