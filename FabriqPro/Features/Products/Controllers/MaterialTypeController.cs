using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/material-types")]
public class MaterialTypeController(MaterialTypeService service) : ControllerBase
{
  [HttpGet("retrieve/{id:int}")]
  public async Task<ActionResult<MaterialTypeDetailDto>> GetMaterialTypeById(int id)
  {
    var materialType = await service.GetMaterialTypeByIdAsync(id);
    return Ok(materialType);
  }

  [HttpGet("list")]
  public async Task<ActionResult<List<MaterialTypeListDto>>> GetAllMaterialTypes()
  {
    var materialTypes = await service.GetAllMaterialTypesAsync();
    return Ok(materialTypes);
  }

  [HttpPost("create")]
  public async Task<ActionResult<MaterialType>> CreateMaterialType(MaterialTypeCreateDto payload)
  {
    var materialType = await service.CreateMaterialTypeAsync(payload);
    return CreatedAtAction(nameof(GetMaterialTypeById), new { id = materialType.Id }, materialType);
  }

  [HttpDelete("delete/{id:int}")]
  public async Task<ActionResult> DeleteMaterialType(int id)
  {
    await service.DeleteMaterialTypeByIdAsync(id);
    return NoContent();
  }
}