using FabriqPro.Features.Products.Controllers.Filters;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Models.Product;
using FabriqPro.Features.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/product-models")]
public class ProductModelController(ProductModelService service) : ControllerBase
{
  [HttpGet("retrieve/{id:int}")]
  public async Task<ActionResult<ProductModelDetailDto>> GetProductModelById(int id)
  {
    var productModel = await service.GetProductModelByIdAsync(id);
    return Ok(productModel);
  }

  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<ProductModelListDto>>> GetProductModels([FromQuery] ProductModelFilters filters)
  {
    var productModels = await service.GetAllProductModelsAsync(filters);
    return Ok(productModels);
  }

  [HttpPost("create")]
  public async Task<ActionResult<ProductModel>> CreateProductModel(ProductModelCreateDto payload)
  {
    var productModel = await service.CreateProductModelAsync(payload);
    return CreatedAtAction(nameof(GetProductModelById), new { id = productModel.Id }, productModel);
  }

  [HttpPatch("update/{id:int}")]
  public async Task<ActionResult<ProductModel>> UpdateProductModel(int id, ProductModelUpdateDto payload)
  {
    var updatedProductModel = await service.UpdateProductModelAsync(id, payload);
    return Ok(updatedProductModel);
  }

  [HttpDelete("delete/{id:int}")]
  public async Task<ActionResult> DeleteProductModel(int id)
  {
    await service.DeleteProductModelByIdAsync(id);
    return NoContent();
  }
}