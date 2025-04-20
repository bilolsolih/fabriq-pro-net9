using AutoMapper;
using FabriqPro.Features.Products.DTOs;
using FabriqPro.Features.Products.Models;
using Microsoft.AspNetCore.Mvc;

namespace FabriqPro.Features.Products.Controllers;

[ApiController, Route("api/v1/materials")]
public class MaterialController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
  // [HttpPost("add-to-storage")]
  // public async Task<ActionResult<MaterialCreateDto>> AddToStorage(MaterialCreateDto payload)
  // {
  //   var newMaterial = mapper.Map<Material>(payload);
  //   var newParty = new Party { Title = payload.PartyNumber };
  // }
}