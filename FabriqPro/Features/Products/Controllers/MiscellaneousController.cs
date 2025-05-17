using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
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
  
  [HttpPatch("update-miscellaneous-type/{id:int}"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<MiscellaneousCreateUpdateDto>> UpdateAccessory(int id, MiscellaneousCreateUpdateDto payload)
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
    var allAccessories = await context.MiscellaneousTypes
      .ProjectTo<MiscellaneousTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
  }

  [HttpGet("list-all-miscellaneous")]
  public async Task<ActionResult<List<MiscellaneousListAllDto>>> ListAllAccessories()
  {
    var allAccessories = await context.Miscellaneous
      .Include(sp => sp.MiscellaneousType)
      .Where(sp => sp.Department == Department.Storage && sp.Status == ItemStatus.AcceptedToStorage)
      .ProjectTo<MiscellaneousListAllDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allAccessories);
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
  public async Task<ActionResult<IEnumerable<MiscellaneousFlowListDto>>> GetAccessoriesSentToMe()
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
  public async Task<ActionResult> AcceptOrRejectSentAccessories(int id, [FromBody] bool accept)
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