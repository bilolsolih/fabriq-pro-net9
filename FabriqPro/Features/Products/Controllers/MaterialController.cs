using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Authentication.Models;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Material;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/materials"), Authorize]
public class MaterialController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  [HttpPost("create-new-material"), Authorize(Policy = "SuperAdmin")]
  public async Task<ActionResult<MaterialCreateDto>> CreateMaterial(MaterialCreateDto payload)
  {
    var alreadyExists = await context.Materials.AnyAsync(m => m.Title.ToLower() == payload.Title.ToLower());
    AlreadyExistsException.ThrowIf(alreadyExists, payload.ToString());

    var newMaterial = mapper.Map<Material>(payload);
    context.Materials.Add(newMaterial);
    await context.SaveChangesAsync();
    return Ok(payload);
  }

  [HttpPost("add-to-storage"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<AddMaterialToStorageDto>> AddToStorage(AddMaterialToStorageDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}. Please, login again.");

    var fromUser = await context.Users.FindAsync(payload.FromUserId);
    DoesNotExistException.ThrowIfNull(fromUser, $"fromUserId: {payload.FromUserId}");

    var material = await context.Materials.FindAsync(payload.MaterialId);
    DoesNotExistException.ThrowIfNull(material, $"materialId: {payload.MaterialId}");

    var color = await context.Colors.FindAsync(payload.ColorId);
    DoesNotExistException.ThrowIfNull(color, $"colorId: {payload.ColorId}");

    var partyAlreadyExists = await context.Parties
      .AnyAsync(p => p.Title.ToLower().Contains(payload.PartyNumber.ToLower()));
    AlreadyExistsException.ThrowIf(partyAlreadyExists, $"party: {payload.PartyNumber}");

    await using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
      var newParty = new Party { Title = payload.PartyNumber };
      context.Parties.Add(newParty);
      await context.SaveChangesAsync();

      var materialToDepartment = await context.MaterialInDepartments.SingleOrDefaultAsync(m =>
        m.Department == Department.Storage &&
        m.AcceptedUserId == user.Id &&
        m.MaterialId == payload.MaterialId &&
        m.PartyId == newParty.Id
      );

      AlreadyExistsException.ThrowIf(materialToDepartment != null, "Material already exists in the department");

      var newMaterialToDepartment = new MaterialToDepartment
      {
        Department = Department.Storage,
        FromUserId = payload.FromUserId,
        AcceptedUserId = user.Id,
        ToUserId = user.Id,
        MaterialId = material.Id,
        PartyId = newParty.Id,
        ColorId = payload.ColorId,
        Thickness = payload.Thickness,
        Width = payload.Width,
        HasPatterns = payload.HasPatterns,
        Quantity = payload.Quantity,
        Unit = payload.Unit,
        Status = ItemStatus.Accepted,
      };

      context.MaterialInDepartments.Add(newMaterialToDepartment);

      await context.SaveChangesAsync();

      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      throw;
    }

    return Ok(payload);
  }

  [HttpGet("types")]
  public async Task<ActionResult<List<MaterialTypeListDto>>> ListAllMaterialTypes()
  {
    var allMaterialTypes = await context.Materials
      .Include(mt => mt.MaterialDepartments)
      .ProjectTo<MaterialTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMaterialTypes);
  }

  [HttpGet("types/{id:int}")]
  public async Task<ActionResult<List<MaterialListDto>>> ListAllMaterials(int id)
  {
    var allMaterials = await context.MaterialInDepartments
      .Include(m => m.AcceptedUser)
      .Include(m => m.Material)
      .Include(m => m.Party)
      .Where(m => m.Department == Department.Storage && m.MaterialId == id)
      .ProjectTo<MaterialListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMaterials);
  }

  [HttpPost("give-to-cutting-master"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<GiveMaterialToMasterDto>> TransferToCuttingMaster(GiveMaterialToMasterDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var fromUser = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(fromUser, $"userId: {userId}. Please, login again.");

    var toUser = await context.Users.FindAsync(payload.CuttingMasterId);
    DoesNotExistException.ThrowIfNull(toUser, $"toUserId: {payload.CuttingMasterId}");

    if (toUser.Role != UserRoles.CuttingMaster)
    {
      return BadRequest("Faqat Kesish Masteriga o'tkazib berish mumkin.");
    }

    var source = await context.MaterialInDepartments.FindAsync(payload.MaterialToDepartmentId);
    DoesNotExistException.ThrowIfNull(source, $"materialToDepartmentId: {payload.MaterialToDepartmentId}");

    if (payload.Quantity > source.Quantity)
    {
      return BadRequest("O'tkazilayapgan material miqdori qolgan material miqdoridan ko'p.");
    }

    var newMaterialToDepartment = source with
    {
      OriginId = source.Id,
      Department = Department.Cutting,
      FromUserId = fromUser.Id,
      ToUserId = toUser.Id,
      Quantity = payload.Quantity,
      Status = ItemStatus.Pending,
    };

    source.Quantity -= payload.Quantity;

    context.MaterialInDepartments.Add(newMaterialToDepartment);
    await context.SaveChangesAsync();

    return Ok(payload);
  }

  [HttpGet("materials-sent-to-me"), Authorize(Policy = "CuttingMaster")]
  public async Task<ActionResult<IEnumerable<MaterialFlowListDto>>> GetMaterialsSentToMe()
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var result = await context.MaterialInDepartments
      .Where(m => m.ToUserId == user.Id)
      .ProjectTo<MaterialFlowListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(result);
  }

  [HttpGet("accept-or-reject-sent-materials/{id:int}"), Authorize(Policy = "CuttingMaster")]
  public async Task<ActionResult> AcceptOrRejectSentMaterials(int id, [FromBody] bool accept)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var material = await context.MaterialInDepartments.SingleOrDefaultAsync(m => m.Id == id);
    DoesNotExistException.ThrowIfNull(material, $"materialInDepartmentId: {id}");

    if (material.ToUserId != user.Id)
    {
      return Forbid("Bu material sizga jo'natilmagan.");
    }

    if (accept)
    {
      material.Status = ItemStatus.Accepted;
    }
    else if (!accept)
    {
      material.Status = ItemStatus.Rejected;
      var originalMaterial = await context.MaterialInDepartments.FindAsync(material.OriginId);
      DoesNotExistException.ThrowIfNull(originalMaterial, "Material rad etildi, Lekin ombordagi ildizi topilmadi.");

      originalMaterial.Quantity += material.Quantity;
    }

    await context.SaveChangesAsync();
    return Ok();
  }

  [HttpPost("return-material-to-storage/{id:int}"), Authorize(Policy = "CuttingMasterOrSuperAdmin")]
  public async Task<ActionResult> SendMaterialBackToStorage(int id, ReturnMaterialDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var toUser = await context.Users.FindAsync(payload.ToUserId);
    DoesNotExistException.ThrowIfNull(toUser, "Mavjud bo'lmagan Xodimga material qaytarilyapti.");

    if (toUser.Role != UserRoles.StorageManager)
    {
      return Forbid("Material faqat Omborxona menejerlariga qaytarilishi mumkin.");
    }

    var material = await context.MaterialInDepartments.SingleOrDefaultAsync(m => m.Id == id && m.Status == ItemStatus.Accepted);
    DoesNotExistException.ThrowIfNull(material, $"materialInDepartmentId: {id}");

    if (user.Role != UserRoles.SuperAdmin || material.ToUserId != user.Id)
    {
      return Forbid("Bu amalni bajarish uchun yoki SuperAdmin yoki materialni qabul qilgan Master bo'lishingiz kerak.");
    }

    if (payload.Quantity > material.Quantity)
    {
      return BadRequest("Qaytarilyapgan material miqdori, Masterda mavjud miqdordan ko'p. Amal imkonsiz.");
    }

    var originMaterial = await context.MaterialInDepartments.FindAsync(material.OriginId);
    DoesNotExistException.ThrowIfNull(
      originMaterial,
      "Material qaytarilmoqchi bo'lindi, lekin ombordagi ildizi topilmadi. Masterga qayerdan o'tkazilgani noma'lum."
    );

    var newMaterialTransfer = material with
    {
      FromUserId = user.Id,
      ToUserId = toUser.Id,
      Quantity = payload.ReturnAll == true ? material.Quantity : payload.Quantity,
      Department = Department.Storage,
      Status = ItemStatus.ReturnedToStorage,
    };

    material.Quantity = payload.ReturnAll == true ? 0 : material.Quantity - payload.Quantity;
    originMaterial.Quantity += payload.ReturnAll == true ? material.Quantity : payload.Quantity;
    context.MaterialInDepartments.Add(newMaterialTransfer);

    await context.SaveChangesAsync();
    return Ok();


    /*
     * accept qilingan bo'lishi kerak
     * masterga tegishli bo'lishi yoki superadmin bo'lishi kerak
     * qisman yoki to'liq qaytarish imkoni bo'lishi kerak
     * qanaqa o'lchov birligida qabul qilingan bo'lsa shunda qaytariladi, shuning uchun birlikni tanlay olmasligi kerak
     * agar omborchi bir nechta bo'lishi mumkin holatlari bo'lsa unda aynan qaysi omborchi ekanini tanlanishi kerak
     *
     */
  }

  /*
   * O'ziga o'tkazilgan va accept qilingan materiallarning har biridan qanchadur miqdordan tanlab kesish va natijada
   * maxsulot qismlarini qo'shish, agar mavjud bo'lsa, ko'paytirish uchun API chiqarish kerak
   */
}