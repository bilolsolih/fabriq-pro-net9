using FabriqPro.Features.ProductParts.DTOs;
using FabriqPro.Features.ProductParts.Models;
using FabriqPro.Features.ProductParts.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.ProductParts.Controllers;

[Route("api/v1/product-part-to-departments")]
public class ProductPartToDepartmentController(ProductPartToDepartmentService service) : ControllerBase
{
    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<ProductPartToDepartmentDetailDto>> GetProductPartToDepartmentById(int id)
    {
        var productPartToDepartment = await service.GetProductPartToDepartmentByIdAsync(id);
        return Ok(productPartToDepartment);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<ProductPartToDepartmentListDto>>> GetProductPartToDepartments()
    {
        var productPartToDepartments = await service.GetAllProductPartToDepartmentsAsync();
        return Ok(productPartToDepartments);
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProductPartToDepartment>> CreateProductPartToDepartment(ProductPartToDepartmentCreateDto payload)
    {
        var productPartToDepartment = await service.CreateProductPartToDepartmentAsync(payload);
        return CreatedAtAction(nameof(GetProductPartToDepartmentById), new { id = productPartToDepartment.Id }, productPartToDepartment);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<ProductPartToDepartment>> UpdateProductPartToDepartment(int id, ProductPartToDepartmentUpdateDto payload)
    {
        var updatedProductPartToDepartment = await service.UpdateProductPartToDepartmentAsync(id, payload);
        return Ok(updatedProductPartToDepartment);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteProductPartToDepartment(int id)
    {
        await service.DeleteProductPartToDepartmentAsync(id);
        return NoContent();
    }
}