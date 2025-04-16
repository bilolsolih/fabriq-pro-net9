using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.ProductParts.Controllers;

[ApiController, Route("api/v1/product-parts")]
public class ProductPartController(ProductPartService service) : ControllerBase
{
    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<ProductPartDetailDto>> GetProductPartById(int id)
    {
        var productPart = await service.GetProductPartByIdAsync(id);
        return Ok(productPart);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<ProductPartListDto>>> GetProductParts()
    {
        var productParts = await service.GetAllProductPartsAsync();
        return Ok(productParts);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProductPart>> CreateProductPart(ProductPartCreateDto payload)
    {
        var productPart = await service.CreateProductPartAsync(payload);
        return CreatedAtAction(nameof(GetProductPartById), new { id = productPart.Id }, productPart);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<ProductPart>> UpdateProductPart(int id, ProductPartUpdateDto payload)
    {
        var updatedProductPart = await service.UpdateProductPartAsync(id, payload);
        return Ok(updatedProductPart);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteProductPart(int id)
    {
        await service.DeleteProductPartByIdAsync(id);
        return NoContent();
    }
}