using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
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

  [HttpGet("list-material-types")]
  public async Task<ActionResult<IEnumerable<MaterialTypeListDto>>> ListMaterialTypes()
  {
    var materials = await context.Materials
      .ProjectTo<MaterialTypeListDto>(mapper.ConfigurationProvider)
      .ToListAsync();
    return Ok(materials);
  }

  [HttpPost("add-to-storage"), Authorize(Policy = "StorageManagerOrSuperAdmin")]
  public async Task<ActionResult<AddMaterialToStorageDto>> AddToStorage(AddMaterialToStorageDto payload)
  {
    var userId = int.Parse(User.FindFirstValue("id")!);
    var user = await context.Users.FindAsync(userId);
    DoesNotExistException.ThrowIfNull(user, $"userId: {userId}");

    var material = await context.Materials.FindAsync(payload.MaterialId);
    DoesNotExistException.ThrowIfNull(material, $"materialId: {payload.MaterialId}");

    var color = await context.Colors.FindAsync(payload.ColorId);
    DoesNotExistException.ThrowIfNull(color, $"colorId: {payload.ColorId}");

    await using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
      var party = await context.Parties
        .SingleOrDefaultAsync(p => p.Title.ToLower().Contains(payload.PartyNumber.ToLower()));
      int partyId;
      if (party == null)
      {
        var newParty = new Party { Title = payload.PartyNumber };
        context.Parties.Add(newParty);
        await context.SaveChangesAsync();
        partyId = newParty.Id;
      }
      else
      {
        partyId = party.Id;
      }

      var materialToDepartment = await context.MaterialInDepartments.SingleOrDefaultAsync(m =>
        m.Department == Department.Storage &&
        m.UserId == user.Id &&
        m.MaterialId == payload.MaterialId &&
        m.PartyId == partyId
      );

      AlreadyExistsException.ThrowIf(materialToDepartment != null, "Material already exists in the department");

      var newMaterialToDepartment = new MaterialToDepartment
      {
        Department = Department.Storage,
        UserId = user.Id,
        MaterialId = material.Id,
        PartyId = partyId,
        ColorId = payload.ColorId,
        Thickness = payload.Thickness,
        Width = payload.Width,
        HasPatterns = payload.HasPatterns,
        Quantity = payload.Quantity,
        Unit = payload.Unit,
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
      .Include(m => m.User)
      .Include(m => m.Material)
      .Include(m => m.Party)
      .Where(m => m.Department == Department.Storage && m.MaterialId == id)
      .ProjectTo<MaterialListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMaterials);
  }
}