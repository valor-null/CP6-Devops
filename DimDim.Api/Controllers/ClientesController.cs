using DimDim.Api.DTOs;
using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _repo;

    public ClientesController(IClienteRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll(CancellationToken ct)
    {
        var data = await _repo.GetAllAsync(ct);
        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetById(int id, CancellationToken ct)
    {
        var cliente = await _repo.GetByIdAsync(id, ct);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Create(ClienteCreateDto dto, CancellationToken ct)
    {
        if (await _repo.ExistsCpfAsync(dto.CPF, null, ct)) return Conflict("CPF já cadastrado.");
        if (await _repo.ExistsEmailAsync(dto.Email, null, ct)) return Conflict("Email já cadastrado.");

        var entity = new Cliente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            Email = dto.Email
        };

        var created = await _repo.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCliente }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClienteUpdateDto dto, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity == null) return NotFound();

        if (!string.Equals(entity.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repo.ExistsEmailAsync(dto.Email, id, ct)) return Conflict("Email já cadastrado.");
        }

        entity.Nome = dto.Nome;
        entity.Email = dto.Email;

        await _repo.UpdateAsync(entity, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var exists = await _repo.GetByIdAsync(id, ct);
        if (exists == null) return NotFound();

        if (await _repo.HasContasAsync(id, ct)) return Conflict("Cliente possui contas vinculadas.");

        await _repo.DeleteAsync(id, ct);
        return NoContent();
    }
}
