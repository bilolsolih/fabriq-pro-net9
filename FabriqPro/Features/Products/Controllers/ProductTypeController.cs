using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;
using FabriqPro.Features.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/product-types")]
public class ProductTypeController(ProductTypeService service) : ControllerBase
{
  [HttpGet("retrieve/{id:int}")]
  public async Task<ActionResult<ProductTypeDetailDto>> GetProductTypeById(int id)
  {
    var productType = await service.GetProductTypeByIdAsync(id);
    return Ok(productType);
  }

  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<ProductTypeListDto>>> GetProductTypes()
  {
    var productTypes = await service.GetAllProductTypesAsync();
    return Ok(productTypes);
  }

  [HttpPost("create")]
  public async Task<ActionResult<ProductType>> CreateProductType(ProductTypeCreateDto payload)
  {
    var productType = await service.CreateProductTypeAsync(payload);
    return CreatedAtAction(nameof(GetProductTypeById), new { id = productType.Id }, productType);
  }

  [HttpPatch("update/{id:int}")]
  public async Task<ActionResult<ProductType>> UpdateProductType(int id, ProductTypeUpdateDto payload)
  {
    var updatedProductType = await service.UpdateProductTypeAsync(id, payload);
    return Ok(updatedProductType);
  }

  [HttpDelete("delete/{id:int}")]
  public async Task<ActionResult> DeleteProductType(int id)
  {
    await service.DeleteProductTypeByIdAsync(id);
    return NoContent();
  }
}