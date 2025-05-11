using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
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
  public async Task<ActionResult<SparePartCreateDto>> CreateSparePart(SparePartCreateDto payload)
  {
    var alreadyExists = await context.SparePartTypes.AnyAsync(sp => sp.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newSparePart = new SparePartType { Title = payload.Title };
    context.SparePartTypes.Add(newSparePart);
    await context.SaveChangesAsync();
    return Ok(payload);
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

    var sparePart = await context.SparePartTypes.FindAsync(payload.SparePartId);
    DoesNotExistException.ThrowIfNull(sparePart, "Omborga mavjud bo'lmagan turdagi 'Ehtiyot qism' qo'shilmoqda.");

    var sparePartDepartment = new SparePart
    {
      Department = Department.Storage,
      FromUserId = payload.FromUserId,
      AcceptedUserId = user.Id,
      ToUserId = user.Id,
      SparePartId = sparePart.Id,
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

  [HttpGet("list-all-spare-parts")]
  public async Task<ActionResult<List<SparePartListDto>>> ListAllSpareParts()
  {
    var allSpareParts = await context.SpareParts
      .Include(sp => sp.AcceptedUser)
      .Include(sp => sp.FromUser)
      .Include(sp => sp.SparePartType)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage)
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
      originSparePart, "Ehtiyot qism qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi."
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