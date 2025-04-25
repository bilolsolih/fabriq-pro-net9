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

      var materialToDepartment = await context.MaterialToDepartments.SingleOrDefaultAsync(m =>
        m.Department == Department.Storage &&
        m.UserId == user.Id &&
        m.MaterialId == payload.MaterialId &&
        m.PartyId == partyId
      );

      if (materialToDepartment != null)
      {
        materialToDepartment.Quantity += payload.Quantity;
      }
      else
      {
        var newMaterialToDepartment = new MaterialToDepartment
        {
          Department = Department.Storage,
          UserId = user.Id,
          MaterialId = material.Id,
          PartyId = partyId,
          Quantity = payload.Quantity,
        };

        context.MaterialToDepartments.Add(newMaterialToDepartment);
      }

      await context.SaveChangesAsync();

      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      return BadRequest();
    }

    return Ok(payload);
  }

  [HttpGet("list-all-materials")]
  public async Task<ActionResult<List<MaterialListDto>>> ListAllMaterials()
  {
    var allMaterials = await context.MaterialToDepartments
      .Include(m => m.User)
      .Include(m => m.Material)
      .Include(m => m.Party)
      .Where(m => m.Department == Department.Storage)
      .ProjectTo<MaterialListDto>(mapper.ConfigurationProvider)
      .ToListAsync();

    return Ok(allMaterials);
  }
}