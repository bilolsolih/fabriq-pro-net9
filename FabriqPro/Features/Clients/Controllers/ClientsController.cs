using AutoMapper;
using AutoMapper.QueryableExtensions;
using FabriqPro.Core.Exceptions;
using FabriqPro.Features.Clients.Controllers.Filters;
using FabriqPro.Features.Clients.DTOs;
using FabriqPro.Features.Clients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FabriqPro.Features.Clients.Controllers;

// TODO: ClientRepository va ClientService ga ajratish

[ApiController, Route("api/v1/clients")]
public class ClientsController(FabriqDbContext context, IMapper mapper) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<ClientCreateDto>> CreateClient(ClientCreateDto payload)
    {
        var newClient = mapper.Map<Client>(payload);

        context.Clients.Add(newClient);
        await context.SaveChangesAsync();
        return StatusCode(201, payload);
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<ClientListDto>>> ListClients([FromQuery] ClientFilters filters)
    {
        var clientsQuery = context.Clients.AsQueryable();

        if (filters is { Search: not null })
        {
            filters.Search = filters.Search.ToLower();
            clientsQuery = clientsQuery.Where(
                client =>
                    client.FirstName.ToLower().Contains(filters.Search) ||
                    client.LastName.ToLower().Contains(filters.Search) ||
                    client.PhoneNumber.Contains(filters.Search)
            );
        }

        if (filters is { Page: not null, Limit: not null })
        {
            var totalCount = Math.Ceiling(clientsQuery.Count() / (double)filters.Limit);
            HttpContext.Response.Headers.Append("totalPages", $"{totalCount}");
            clientsQuery = clientsQuery.Skip((int)(filters.Limit * (filters.Page - 1)));
        }

        if (filters is { Limit: not null })
        {
            clientsQuery = clientsQuery.Take((int)filters.Limit);
        }

        var clients = await clientsQuery.ProjectTo<ClientListDto>(mapper.ConfigurationProvider).ToListAsync();

        return Ok(clients);
    }

    [HttpGet("retrieve/{id:int}")]
    public async Task<ActionResult<ClientDetailDto>> RetrieveClient(int id)
    {
        var client = await context.Clients.ProjectTo<ClientDetailDto>(mapper.ConfigurationProvider)
            .SingleAsync(client => client.Id == id);

        return Ok(client);
    }

    [HttpPatch("update/{id:int}")]
    public async Task<ActionResult<Client>> UpdateClient(int id,ClientUpdateDto payload)
    {
        var client = await context.Clients.FindAsync(id);
        DoesNotExistException.ThrowIfNull(client, nameof(Client));

        mapper.Map(payload, client);
        client.Updated = DateTime.UtcNow;
        await context.SaveChangesAsync();
        client.Updated = client.Updated.ToLocalTime();
        client.Created = client.Created.ToLocalTime();
        return Ok(client);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var client = await context.Clients.FindAsync(id);
        DoesNotExistException.ThrowIfNull(client, nameof(Client));

        context.Clients.Remove(client);
        await context.SaveChangesAsync();
        return NoContent();
    }
}