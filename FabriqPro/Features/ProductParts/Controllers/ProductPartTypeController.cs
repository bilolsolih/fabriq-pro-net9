using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.ProductParts.Controllers;

[ApiController, Route("api/v1/product-part-types")]
public class ProductPartTypeController(ProductPartTypeService service) : ControllerBase
{
    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<ProductPartTypeDetailDto>> GetProductPartTypeById(int id)
    {
        var productPartType = await service.GetProductPartTypeByIdAsync(id);
        return Ok(productPartType);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<ProductPartTypeListDto>>> GetProductPartTypes()
    {
        var productPartTypes = await service.GetAllProductPartTypesAsync();
        return Ok(productPartTypes);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProductPartType>> CreateProductPartType(ProductPartTypeCreateDto payload)
    {
        var productPartType = await service.CreateProductPartTypeAsync(payload);
        return CreatedAtAction(nameof(GetProductPartTypeById), new { id = productPartType.Id }, productPartType);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<ProductPartType>> UpdateProductPartType(int id, ProductPartTypeUpdateDto payload)
    {
        var updatedProductPartType = await service.UpdateProductPartTypeAsync(id, payload);
        return Ok(updatedProductPartType);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteProductPartType(int id)
    {
        await service.DeleteProductPartTypeAsync(id);
        return NoContent();
    }
}