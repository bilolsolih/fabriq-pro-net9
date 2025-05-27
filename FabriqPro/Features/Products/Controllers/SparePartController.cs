using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.Controllers.Filters;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.SparePart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/spare-parts"), Authorize]
public class SparePartController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  [HttpPost("create-new-spare-part-type"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<SparePartCreateUpdateDto>> CreateSparePart(SparePartCreateUpdateDto payload)
  {
    var alreadyExists = await context.SparePartTypes.AnyAsync(sp => sp.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newSparePart = new SparePartType { Title = payload.Title };
    context.SparePartTypes.Add(newSparePart);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpPatch("update-spare-part/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> UpdateSparePart(int id, SparePartUpdateDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var sparePart = await context.SpareParts.FindAsync(id);
    DoesNotExistException.ThrowIfNull(sparePart, "Bunday ehtiyot qism mavjud emas.");

    if (payload is { FromUserId: not null } && payload.FromUserId != sparePart.FromUserId)
    {
      var fromUser = await context.Users.FindAsync(payload.FromUserId);
      DoesNotExistException.ThrowIfNull(fromUser, "Bunday foydalanuvchi mavjud emas.");

      if (fromUser.Role != UserRoles.Supplier)
      {
        return Forbid("Faqat yetkazib beruvchi tanlanishi mumkin.");
      }

      sparePart.FromUserId = (int)payload.FromUserId;
    }

    if (payload is { SparePartTypeId: not null } && payload.SparePartTypeId != sparePart.SparePartTypeId)
    {
      var sparePartType = await context.SparePartTypes.FindAsync(payload.SparePartTypeId);
      DoesNotExistException.ThrowIfNull(sparePartType, "Bunday ehtiyot qism turi mavjud emas.");

      sparePart.SparePartTypeId = (int)payload.SparePartTypeId;
    }

    if (payload is { Quantity: not null } && !(Math.Abs(sparePart.Quantity - (double)payload.Quantity) < 1e-10))
    {
      sparePart.Quantity = (double)payload.Quantity;
    }

    if (payload is { Unit: not null } && sparePart.Unit != payload.Unit)
    {
      sparePart.Unit = (Unit)payload.Unit;
    }

    context.SpareParts.Update(sparePart);
    await context.SaveChangesAsync();
    return Ok();
  }
  
  [HttpDelete("delete-spare-part/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteSparePart(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var sparePart = await context.SpareParts.FindAsync(id);
    DoesNotExistException.ThrowIfNull(sparePart, "Bunday ehtiyot qism mavjud emas.");

    context.SpareParts.Remove(sparePart);
    await context.SaveChangesAsync();
    return NoContent();
  }

  [HttpPatch("update-spare-part-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<SparePartCreateUpdateDto>> UpdateSparePartType(int id, SparePartCreateUpdateDto payload)
  {
    var sparePartType = await context.SparePartTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(sparePartType, "O'zgartirmoqchi bo'lingan ehtiyot qism turi mavjud emas.");

    var alreadyExists = await context.SparePartTypes.AnyAsync(s => s.Title.ToLower() == payload.Title.ToLower() && s.Id != id);
    AlreadyExistsException.ThrowIf(alreadyExists, "Bunday nom bilan boshqa ehtiyot qism mavjud. Boshqa nom tanlang.");

    sparePartType.Title = payload.Title;
    context.SparePartTypes.Update(sparePartType);

    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpDelete("delete-spare-part-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult> DeleteSparePartType(int id)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, "Qaytadan login qilib yana urinib ko'ring.");

    var sparePartType = await context.SparePartTypes.FindAsync(id);
    DoesNotExistException.ThrowIfNull(sparePartType, "Bunday ehtiyot qism turi mavjud emas.");

    var hasAnySpareParts = await context.SpareParts.AnyAsync(m => m.SparePartTypeId == sparePartType.Id);
    if (hasAnySpareParts)
    {
      return BadRequest("Bu ehtiyot qism turiga bog'langan ehtiyot qismlar mavjud, o'chirish mumkin emas.");
    }

    context.SparePartTypes.Remove(sparePartType);
    await context.SaveChangesAsync();
    return NoContent();
  }

  [HttpPost("accept-spare-part-to-storage"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<AddSparePartToStorageDto>> AddToStorage(AddSparePartToStorageDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Please, login again.");

    var fromUser = await context.Users.FindAsync(payload.FromUserId);
    DoesNotExistException.ThrowIfNull(fromUser, $"fromUserId: {payload.FromUserId}");

    if (fromUser.Role != UserRoles.Supplier)
    {
      return Forbid("Ehtiyot qism faqat yetkazib beruvchidan qabul qilib olinishi mumkin.");
    }

    var sparePart = await context.SparePartTypes.FindAsync(payload.SparePartTypeId);
    DoesNotExistException.ThrowIfNull(sparePart, "Omborga mavjud bo'lmagan turdagi 'Ehtiyot qism' qo'shilmoqda.");

    var sparePartDepartment = new SparePart
    {
      Department = Department.Storage,
      FromUserId = payload.FromUserId,
      AcceptedUserId = user.Id,
      ToUserId = user.Id,
      SparePartTypeId = sparePart.Id,
      Quantity = payload.Quantity,
      Unit = payload.Unit,
      Status = ItemStatus.AcceptedToStorage,
    };

    context.SpareParts.Add(sparePartDepartment);

    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("list-all-spare-part-types")]
  public async Task<ActionResult<List<SparePartTypeListDto>>> ListAllSparePartTypes()
  {
    var allSpareParts = await context.SparePartTypes
      .ProjectTo<SparePartTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allSpareParts);
  }

  [HttpGet("list-all-spare-part-entries")]
  public async Task<ActionResult<List<SparePartTypeEntryListDto>>> ListAllSparePartEntries()
  {
    var allSparePartEntries = await context.SparePartTypes
      .ProjectTo<SparePartTypeEntryListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allSparePartEntries);
  }

  [HttpGet("list-all-spare-parts")]
  public async Task<ActionResult<List<SparePartListDto>>> ListAllSpareParts([FromQuery] SparePartFilters filters)
  {
    var query = context.SpareParts
      .Include(sp => sp.AcceptedUser)
      .Include(sp => sp.FromUser)
      .Include(sp => sp.SparePartType)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage);

    if (filters is { TypeId: not null })
    {
      query = query.Where(a => a.SparePartTypeId == filters.TypeId);
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

    var allSpareParts = await query
      .OrderBy(s => s.SparePartType.Title)
      .ProjectTo<SparePartListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allSpareParts);
  }

  [HttpPost("give-to-master"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<GiveSparePartToMasterDto>> TransferToMaster(GiveSparePartToMasterDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var fromUser = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(fromUser, $"userId: {userId}. Please, login again.");

    var toUser = await context.Users.FindAsync(payload.MasterId);
    DoesNotExistException.ThrowIfNull(toUser, $"toUserId: {payload.MasterId}");

    if (toUser is not { Role: UserRoles.CuttingMaster or UserRoles.PackagingMaster or UserRoles.SewingMaster })
    {
      return BadRequest("Faqat Masterga o'tkazib berish mumkin.");
    }

    var source = await context.SpareParts.FindAsync(payload.SparePartToDepartmentId);
    DoesNotExistException.ThrowIfNull(source, $"sparePartDepartmentId: {payload.SparePartToDepartmentId}");

    if (payload.Quantity > source.Quantity)
    {
      return BadRequest("O'tkazilayapgan Ehtiyot qism miqdori mavjud miqdordan ko'p.");
    }

    var newSparePartToDepartment = source with
    {
      Department = payload.Department,
      FromUserId = fromUser.Id,
      ToUserId = toUser.Id,
      Quantity = payload.Quantity,
      Status = ItemStatus.Pending,
    };

    source.Quantity -= payload.Quantity;

    context.SpareParts.Add(newSparePartToDepartment);
    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("spare-parts-sent-to-me"), Authorize(Policy = "Master")]
  public async Task<ActionResult<IEnumerable<SparePartFlowListDto>>> GetSparePartsSentToMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var result = await context.SpareParts
      .Where(m => m.ToUserId == user.Id)
      .ProjectTo<SparePartFlowListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(result);
  }

  [HttpGet("accept-or-reject-sent-spare-parts/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> AcceptOrRejectSentSpareParts(int id, [FromBody] bool accept)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var sparePart = await context.SpareParts.SingleOrDefaultAsync(sp => sp.Id == id);
    DoesNotExistException.ThrowIfNull(sparePart, $"sparePartInDepartmentId: {id}");

    if (sparePart.Status != ItemStatus.Pending)
    {
      return Forbid("Bu ehtiyot qism bilan ortiq bu amalni bajarib bo'lmaydi. U qabul/rad qilib bo'lingan.");
    }

    if (sparePart.ToUserId != user.Id)
    {
      return Forbid("Bu ehtiyot qism sizga jo'natilmagan.");
    }

    if (accept)
    {
      sparePart.Status = ItemStatus.Accepted;
    }
    else if (!accept)
    {
      sparePart.Status = ItemStatus.Rejected;
      var originalSparePart = await context.SpareParts.FindAsync(sparePart.OriginId);
      DoesNotExistException.ThrowIfNull(originalSparePart, "Ehtiyot qism rad etildi, lekin ombordagi ildizi topilmadi.");

      originalSparePart.Quantity += sparePart.Quantity;
    }

    await context.SaveChangesAsync();
    return Ok();
  }

  [HttpPost("return-spare-part-to-storage/{id:int}"), Authorize(Policy = "Master")]
  public async Task<ActionResult> ReturnSparePartToStorage(int id, ReturnSparePartDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var toUser = await context.Users.FindAsync(payload.ToUserId);
    DoesNotExistException.ThrowIfNull(toUser, "Mavjud bo'lmagan Xodimga ehtiyot qism qaytarilyapti.");

    if (toUser.Role != UserRoles.StorageManager)
    {
      return Forbid("Ehtiyot qism faqat Omborxona menejerlariga qaytarilishi mumkin.");
    }

    var sparePart = await context.SpareParts.SingleOrDefaultAsync(m => m.Id == id);
    DoesNotExistException.ThrowIfNull(sparePart, $"materialInDepartmentId: {id}");

    if (sparePart.Status != ItemStatus.Accepted)
    {
      return Forbid("Ehtiyot qism 'Qabul qilingan' statusida emas.");
    }

    if (user.Role != UserRoles.SuperAdmin || sparePart.ToUserId != user.Id)
    {
      return Forbid("Bu amalni bajarish uchun yoki SuperAdmin yoki ehtiyot qismni qabul qilgan Master bo'lishingiz kerak.");
    }

    if (payload.Quantity > sparePart.Quantity)
    {
      return BadRequest("Qaytarilyapgan ehtiyot qism miqdori Masterda mavjud miqdordan ko'p. Amal imkonsiz.");
    }

    var originSparePart = await context.SpareParts.FindAsync(sparePart.OriginId);
    DoesNotExistException.ThrowIfNull(
      originSparePart,
      "Ehtiyot qism qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi."
    );

    var newSparePartTransfer = sparePart with
    {
      FromUserId = user.Id,
      ToUserId = toUser.Id,
      Quantity = payload.ReturnAll == true ? sparePart.Quantity : payload.Quantity,
      Department = Department.Storage,
      Status = ItemStatus.ReturnedToStorage,
    };

    sparePart.Quantity = payload.ReturnAll == true ? 0 : sparePart.Quantity - payload.Quantity;
    originSparePart.Quantity += payload.ReturnAll == true ? sparePart.Quantity : payload.Quantity;
    context.SpareParts.Add(newSparePartTransfer);
    await context.SaveChangesAsync();
    return Ok();
  }
}