using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Controllers.Filters;
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
  public async Task<ActionResult<AccessoryCreateUpdateDto>> CreateAccessory(AccessoryCreateUpdateDto payload)
  {
    var alreadyExists = await context.AccessoryTypes.AnyAsync(sp => sp.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newAccessory = new AccessoryType { Title = payload.Title };
    context.AccessoryTypes.Add(newAccessory);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpPatch("update-accessory-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<AccessoryCreateUpdateDto>> UpdateAccessory(int id, AccessoryCreateUpdateDto payload)
  {
    var accessoryType = await context.AccessoryTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(accessoryType, "O'zgartirmoqchi bo'lingan aksessuar turi mavjud emas.");

    var alreadyExists = await context.AccessoryTypes.AnyAsync(a => a.Title.ToLower() == payload.Title.ToLower() && a.Id != id);
    AlreadyExistsException.ThrowIf(alreadyExists, "Bunday nom bilan boshqa aksessuar mavjud. Boshqa nom tanlang.");

    accessoryType.Title = payload.Title;
    context.AccessoryTypes.Update(accessoryType);

    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpDelete("delete-accessory-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteAccessoryType(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var accessoryType = await context.AccessoryTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(accessoryType, "Bunday aksessuar mavjud emas.");

    var hasAnyAccessories = await context.Accessories.AnyAsync(m => m.AccessoryTypeId == accessoryType.Id);
    if (hasAnyAccessories)
    {
      return BadRequest("Bu aksessuar turiga bog'langan aksessuarlar mavjud, o'chirish mumkin emas.");
    }

    context.AccessoryTypes.Remove(accessoryType);
    await context.SaveChangesAsync();
    return NoContent();
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

    var accessoryDepartment = new Accessory
    {
      Department = Department.Storage,
      FromUserId = payload.FromUserId,
      AcceptedUserId = user.Id,
      ToUserId = user.Id,
      AccessoryTypeId = accessory.Id,
      Quantity = payload.Quantity,
      Unit = payload.Unit,
      Status = ItemStatus.AcceptedToStorage,
    };

    context.Accessories.Add(accessoryDepartment);

    await context.SaveChangesAsync();

    return Ok(payload);
  }
  
  [HttpPatch("update-accessory/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> UpdateAccessory(int id, AccessoryUpdateDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var accessory = await context.Accessories.FindAsync(id);
    DoesNotExistException.ThrowIfNull(accessory, "Bunday aksessuar mavjud emas.");

    if (payload is { FromUserId: not null } && payload.FromUserId != accessory.FromUserId)
    {
      var fromUser = await context.Users.FindAsync(payload.FromUserId);
      DoesNotExistException.ThrowIfNull(fromUser, "Bunday foydalanuvchi mavjud emas.");

      if (fromUser.Role != UserRoles.Supplier)
      {
        return Forbid("Faqat yetkazib beruvchi tanlanishi mumkin.");
      }

      accessory.FromUserId = (int)payload.FromUserId;
    }

    if (payload is { AccessoryTypeId: not null } && payload.AccessoryTypeId != accessory.AccessoryTypeId)
    {
      var sparePartType = await context.AccessoryTypes.FindAsync(payload.AccessoryTypeId);
      DoesNotExistException.ThrowIfNull(sparePartType, "Bunday ehtiyot qism turi mavjud emas.");

      accessory.AccessoryTypeId = (int)payload.AccessoryTypeId;
    }

    if (payload is { Quantity: not null } && !(Math.Abs(accessory.Quantity - (double)payload.Quantity) < 1e-10))
    {
      accessory.Quantity = (double)payload.Quantity;
    }

    if (payload is { Unit: not null } && accessory.Unit != payload.Unit)
    {
      accessory.Unit = (Unit)payload.Unit;
    }

    context.Accessories.Update(accessory);
    await context.SaveChangesAsync();
    return Ok();
  }

  [HttpDelete("delete-accessory/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteAccessory(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var accessory = await context.Accessories.FindAsync(id);
    DoesNotExistException.ThrowIfNull(accessory, "Bunday aksessuar mavjud emas.");

    context.Accessories.Remove(accessory);
    await context.SaveChangesAsync();
    return NoContent();
  }
  
  [HttpGet("list-all-accessory-types")]
  public async Task<ActionResult<List<AccessoryTypeListDto>>> ListAllAccessoryTypes()
  {
    var allAccessories = await context.AccessoryTypes
      .Include(a => a.Accessories)
      .ProjectTo<AccessoryTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
  }
  
  [HttpGet("list-all-accessory-entries")]
  public async Task<ActionResult<List<AccessoryTypeEntryListDto>>> ListAllAccessoryEntries()
  {
    var allAccessories = await context.AccessoryTypes
      .ProjectTo<AccessoryTypeEntryListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
  }

  [HttpGet("list-all-accessories")]
  public async Task<ActionResult<List<AccessoryListDto>>> ListAllAccessories([FromQuery] AccessoryFilters filters)
  {
    var query = context.Accessories
      .Include(sp => sp.AcceptedUser)
      .Include(sp => sp.FromUser)
      .Include(sp => sp.AccessoryType)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage);

    if (filters is { TypeId: not null })
    {
      query = query.Where(a => a.AccessoryTypeId == filters.TypeId);
    }

    if (filters is { FromUserId: not null })
    {
      query = query.Where(a => a.FromUserId == filters.FromUserId);
    }

    if (filters is { ToUserId: not null })
    {
      query = query.Where(a => a.ToUserId == filters.ToUserId);
    }

    if (filters is { StartDate: not null })
    {
      query = query.Where(a => a.Created >= filters.StartDate);
    }

    if (filters is { EndDate: not null })
    {
      query = query.Where(a => a.Created <= filters.EndDate);
    }

    if (filters is { Status: not null })
    {
      query = query.Where(a => a.Status == filters.Status);
    }

    if (filters is { Limit: not null, Page: not null })
    {
      var itemsCount = await query.CountAsync();
      var pagesCount = itemsCount / filters.Limit;

      query = query.Skip((int)((filters.Page - 1) * filters.Limit)).Take((int)filters.Limit);
      HttpContext.Response.Headers.Append("X-PagesCount", pagesCount.ToString());
    }

    var allAccessories = await query.ProjectTo<AccessoryListDto>(mapper.ConfigurationProvider).ToListAsync();

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
      originAccessory,
      "Aksessuar qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi."
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