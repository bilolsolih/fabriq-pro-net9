// using FabriqPro.Features.Products.DTOs;
// using FabriqPro.Features.Products.Models;
// using FabriqPro.Features.Products.Services;
// using Microsoft.AspNetCore.Mvc;
//
// namespace FabriqPro.Features.Products.Controllers;
//
// [ApiController, Route("api/v1/departments")]
// public class DepartmentController(DepartmentService service) : ControllerBase
// {
//   [HttpGet("retrieve/{id:int}")]
//   public async Task<ActionResult<DepartmentDetailDto>> GetDepartmentById(int id)
//   {
//     var department = await service.GetDepartmentByIdAsync(id);
//     return Ok(department);
//   }
//
//   [HttpGet("list")]
//   public async Task<ActionResult<IEnumerable<DepartmentListDto>>> GetDepartments()
//   {
//     var departments = await service.GetAllDepartmentsAsync();
//     return Ok(departments);
//   }
//
//   [HttpPost("create")]
//   public async Task<ActionResult<Department>> CreateDepartment(DepartmentCreateDto payload)
//   {
//     var department = await service.CreateDepartmentAsync(payload);
//     return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
//   }
//
//   [HttpPatch("update/{id:int}")]
//   public async Task<ActionResult<Department>> UpdateDepartment(int id, DepartmentUpdateDto payload)
//   {
//     var updatedDepartment = await service.UpdateDepartmentAsync(id, payload);
//     return Ok(updatedDepartment);
//   }
//
//   [HttpDelete("delete/{id:int}")]
//   public async Task<ActionResult> DeleteDepartment(int id)
//   {
//     await service.DeleteDepartmentByIdAsync(id);
//     return NoContent();
//   }
// }