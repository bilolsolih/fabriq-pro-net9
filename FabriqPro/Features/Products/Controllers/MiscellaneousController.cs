using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Controllers.Filters;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Miscellaneous;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/miscellaneous"), Authorize]
public class MiscellaneousController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  [HttpPost("create-new-miscellaneous-type"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<MiscellaneousCreateUpdateDto>> CreateMiscellaneous(MiscellaneousCreateUpdateDto payload)
  {
    var alreadyExists = await context.MiscellaneousTypes.AnyAsync(sp => sp.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newMiscellaneous = new MiscellaneousType { Title = payload.Title };
    context.MiscellaneousTypes.Add(newMiscellaneous);
    await context.SaveChangesAsync();
    return Ok(payload);
  }
  
  [HttpPatch("update-miscellaneous/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> UpdateMiscellaneous(int id, MiscellaneousUpdateDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var miscellaneous = await context.Miscellaneous.FindAsync(id);
    DoesNotExistException.ThrowIfNull(miscellaneous, "Bunday ehtiyot qism mavjud emas.");

    if (payload is { FromUserId: not null } && payload.FromUserId != miscellaneous.FromUserId)
    {
      var fromUser = await context.Users.FindAsync(payload.FromUserId);
      DoesNotExistException.ThrowIfNull(fromUser, "Bunday foydalanuvchi mavjud emas.");

      if (fromUser.Role != UserRoles.Supplier)
      {
        return Forbid("Faqat yetkazib beruvchi tanlanishi mumkin.");
      }

      miscellaneous.FromUserId = (int)payload.FromUserId;
    }

    if (payload is { MiscellaneousTypeId: not null } && payload.MiscellaneousTypeId != miscellaneous.MiscellaneousTypeId)
    {
      var miscellaneousType = await context.MiscellaneousTypes.FindAsync(payload.MiscellaneousTypeId);
      DoesNotExistException.ThrowIfNull(miscellaneousType, "Bunday ehtiyot qism turi mavjud emas.");

      miscellaneous.MiscellaneousTypeId = (int)payload.MiscellaneousTypeId;
    }

    if (payload is { Quantity: not null } && !(Math.Abs(miscellaneous.Quantity - (double)payload.Quantity) < 1e-10))
    {
      miscellaneous.Quantity = (double)payload.Quantity;
    }

    if (payload is { Unit: not null } && miscellaneous.Unit != payload.Unit)
    {
      miscellaneous.Unit = (Unit)payload.Unit;
    }

    context.Miscellaneous.Update(miscellaneous);
    await context.SaveChangesAsync();
    return Ok();
  }
  
  [HttpDelete("delete-miscellaneous/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteMiscellaneous(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var miscellaneous = await context.Miscellaneous.FindAsync(id);
    DoesNotExistException.ThrowIfNull(miscellaneous, "Bunday narsa mavjud emas.");

    context.Miscellaneous.Remove(miscellaneous);
    await context.SaveChangesAsync();
    return NoContent();
  }
  
  [HttpPatch("update-miscellaneous-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<MiscellaneousCreateUpdateDto>> UpdateMiscellaneous(int id, MiscellaneousCreateUpdateDto payload)
  {
    var miscellaneousType = await context.MiscellaneousTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(miscellaneousType, "O'zgartirmoqchi bo'lingan narsa turi mavjud emas.");
    
    var alreadyExists = await context.MiscellaneousTypes.AnyAsync(m => m.Title.ToLower() == payload.Title.ToLower() && m.Id != id);
    AlreadyExistsException.ThrowIf(alreadyExists, "Bunday nom bilan boshqa narsa mavjud. Boshqa nom tanlang.");

    miscellaneousType.Title = payload.Title;
    context.MiscellaneousTypes.Update(miscellaneousType);
    
    await context.SaveChangesAsync();
    return Ok(payload);
  }
  
  [HttpDelete("delete-miscellaneous-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteMiscellaneousType(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var miscellaneousType = await context.MiscellaneousTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(miscellaneousType, "Bunday narsa mavjud emas.");

    var hasAnyMiscellaneous = await context.Miscellaneous.AnyAsync(m => m.MiscellaneousTypeId == miscellaneousType.Id);
    if (hasAnyMiscellaneous)
    {
      return BadRequest("Bu narsa turiga bog'langan narsalar mavjud, o'chirish mumkin emas.");
    }

    context.MiscellaneousTypes.Remove(miscellaneousType);
    await context.SaveChangesAsync();
    return NoContent();
  }
  
  [HttpPost("accept-miscellaneous-to-storage"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<AddMiscellaneousToStorageDto>> AddToStorage(AddMiscellaneousToStorageDto payload)
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

    var miscellaneousType = await context.MiscellaneousTypes.FindAsync(payload.MiscellaneousId);
    DoesNotExistException.ThrowIfNull(miscellaneousType, "Omborga mavjud bo'lmagan turdagi narsa qo'shilmoqda.");

    var miscellaneousDepartment = new Miscellaneous
    {
      Department = Department.Storage,
      FromUserId = payload.FromUserId,
      AcceptedUserId = user.Id,
      ToUserId = user.Id,
      MiscellaneousTypeId = miscellaneousType.Id,
      Quantity = payload.Quantity,
      Unit = payload.Unit,
      Status = ItemStatus.AcceptedToStorage,
    };

    context.Miscellaneous.Add(miscellaneousDepartment);

    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("list-all-miscellaneous-types")]
  public async Task<ActionResult<List<MiscellaneousTypeListDto>>> ListAllMiscellaneousTypes()
  {
    var allMiscellaneous = await context.MiscellaneousTypes
      .ProjectTo<MiscellaneousTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMiscellaneous);
  }
  
  [HttpGet("list-all-miscellaneous-entries")]
  public async Task<ActionResult<List<MiscellaneousTypeEntryListDto>>> ListAllMiscellaneousEntries()
  {
    var allMiscellaneousEntries = await context.MiscellaneousTypes
      .ProjectTo<MiscellaneousTypeEntryListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMiscellaneousEntries);
  }
  
  [HttpGet("list-all-miscellaneous")]
  public async Task<ActionResult<IEnumerable<MiscellaneousListAllDto>>> ListAllMiscellaneous([FromQuery] MiscellaneousFilters filters)
  {
    var query = context.Miscellaneous
      .Include(sp => sp.AcceptedUser)
      .Include(sp => sp.FromUser)
      .Include(sp => sp.MiscellaneousType)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage);

    if (filters is { TypeId: not null })
    {
      query = query.Where(a => a.MiscellaneousTypeId == filters.TypeId);
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

    var allMiscellaneous = await query.ProjectTo<MiscellaneousListDto>(mapper.ConfigurationProvider).ToListAsync();

    return Ok(allMiscellaneous);
  }

  [HttpPost("give-to-master"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<GiveMiscellaneousToMasterDto>> TransferToMaster(GiveMiscellaneousToMasterDto payload)
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

    var source = await context.Accessories.FindAsync(payload.MiscellaneousToDepartmentId);
    DoesNotExistException.ThrowIfNull(source, $"miscellaneousDepartmentId: {payload.MiscellaneousToDepartmentId}");

    if (payload.Quantity > source.Quantity)
    {
      return BadRequest("O'tkazilayapgan narsa miqdori mavjud miqdordan ko'p.");
    }

    var newMiscellaneousToDepartment = source with
    {
      Department = payload.Department,
      FromUserId = fromUser.Id,
      ToUserId = toUser.Id,
      Quantity = payload.Quantity,
      Status = ItemStatus.Pending,
    };

    source.Quantity -= payload.Quantity;

    context.Accessories.Add(newMiscellaneousToDepartment);
    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("miscellaneous-sent-to-me"), Authorize(Policy = "Master")]
  public async Task<ActionResult<IEnumerable<MiscellaneousFlowListDto>>> GetMiscellaneousSentToMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var result = await context.Accessories
      .Where(m => m.ToUserId == user.Id)
      .ProjectTo<MiscellaneousFlowListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(result);
  }
  
  [HttpGet("accept-or-reject-sent-miscellaneous/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> AcceptOrRejectSentMiscellaneous(int id, [FromBody] bool accept)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var miscellaneous = await context.Accessories.SingleOrDefaultAsync(sp => sp.Id == id);
    DoesNotExistException.ThrowIfNull(miscellaneous, $"miscellaneousInDepartmentId: {id}");

    if (miscellaneous.Status != ItemStatus.Pending)
    {
      return Forbid("Bu narsa bilan ortiq bu amalni bajarib bo'lmaydi. U qabul/rad qilib bo'lingan.");
    }

    if (miscellaneous.ToUserId != user.Id)
    {
      return Forbid("Bu narsa sizga jo'natilmagan.");
    }

    if (accept)
    {
      miscellaneous.Status = ItemStatus.Accepted;
    }
    else if (!accept)
    {
      miscellaneous.Status = ItemStatus.Rejected;
      var originalMiscellaneous = await context.Accessories.FindAsync(miscellaneous.OriginId);
      DoesNotExistException.ThrowIfNull(originalMiscellaneous, "Narsa rad etildi, lekin ombordagi ildizi topilmadi.");

      originalMiscellaneous.Quantity += miscellaneous.Quantity;
    }

    await context.SaveChangesAsync();
    return Ok();
  }

  [HttpPost("return-miscellaneous-to-storage/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> ReturnMiscellaneousToStorage(int id, ReturnMiscellaneousDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Yangittan login qilib qayta urinib ko'ring.");

    var toUser = await context.Users.FindAsync(payload.ToUserId);
    DoesNotExistException.ThrowIfNull(toUser, "Mavjud bo'lmagan Xodimga ehtiyot qism qaytarilyapti.");

    if (toUser.Role != UserRoles.StorageManager)
    {
      return Forbid("Narsa faqat Omborxona menejerlariga qaytarilishi mumkin.");
    }

    var miscellaneous = await context.Miscellaneous.SingleOrDefaultAsync(m => m.Id == id);
    DoesNotExistException.ThrowIfNull(miscellaneous, $"materialInDepartmentId: {id}");

    if (miscellaneous.Status != ItemStatus.Accepted)
    {
      return Forbid("Narsa 'Qabul qilingan' statusida emas.");
    }

    if (user.Role != UserRoles.SuperAdmin || miscellaneous.ToUserId != user.Id)
    {
      return Forbid("Bu amalni bajarish uchun yoki SuperAdmin yoki narsani qabul qilgan Master bo'lishingiz kerak.");
    }

    if (payload.Quantity > miscellaneous.Quantity)
    {
      return BadRequest("Qaytarilyapgan narsa miqdori Masterda mavjud miqdordan ko'p. Amal imkonsiz.");
    }

    var originMiscellaneous = await context.Accessories.FindAsync(miscellaneous.OriginId);
    DoesNotExistException.ThrowIfNull(
      originMiscellaneous, "Narsa qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi."
    );

    var newMiscellaneousTransfer = miscellaneous with
    {
      FromUserId = user.Id,
      ToUserId = toUser.Id,
      Quantity = payload.ReturnAll == true ? miscellaneous.Quantity : payload.Quantity,
      Department = Department.Storage,
      Status = ItemStatus.ReturnedToStorage,
    };

    miscellaneous.Quantity = payload.ReturnAll == true ? 0 : miscellaneous.Quantity - payload.Quantity;
    originMiscellaneous.Quantity += payload.ReturnAll == true ? miscellaneous.Quantity : payload.Quantity;
    context.Miscellaneous.Add(newMiscellaneousTransfer);
    await context.SaveChangesAsync();
    return Ok();
  }
}