using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using FabriqPro.Features.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/colors")]
public class ColorController(ColorService service) : ControllerBase
{
  [HttpGet("retrieve/{id:int}")]
  public async Task<ActionResult<ColorDetailDto>> GetColorById(int id)
  {
    var color = await service.GetColorByIdAsync(id);
    return Ok(color);
  }

  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<ColorListDto>>> GetColors()
  {
    var colors = await service.GetAllColorsAsync();
    return Ok(colors);
  }

  [HttpPost("create")]
  public async Task<ActionResult<Color>> CreateColor(ColorCreateDto payload)
  {
    var color = await service.CreateColorAsync(payload);
    return CreatedAtAction(nameof(GetColorById), new { id = color.Id }, color);
  }

  [HttpPatch("update/{id:int}")]
  public async Task<ActionResult<Color>> UpdateColor(int id, ColorUpdateDto payload)
  {
    var updatedColor = await service.UpdateColorAsync(id, payload);
    return Ok(updatedColor);
  }

  [HttpDelete("delete/{id:int}")]
  public async Task<ActionResult> DeleteColor(int id)
  {
    await service.DeleteColorByIdAsync(id);
    return NoContent();
  }
}