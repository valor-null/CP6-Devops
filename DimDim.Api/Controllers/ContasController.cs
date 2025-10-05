using DimDim.Api.DTOs;
using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly IContaCorrenteRepository _contas;
    private readonly IClienteRepository _clientes;

    public ContasController(IContaCorrenteRepository contas, IClienteRepository clientes)
    {
        _contas = contas;
        _clientes = clientes;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContaCorrente>>> GetAll(CancellationToken ct)
    {
        var data = await _contas.GetAllAsync(ct);
        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContaCorrente>> GetById(int id, CancellationToken ct)
    {
        var conta = await _contas.GetByIdAsync(id, ct);
        if (conta == null) return NotFound();
        return Ok(conta);
    }

    [HttpPost]
    public async Task<ActionResult<ContaCorrente>> Create(ContaCreateDto dto, CancellationToken ct)
    {
        if (!string.Equals(dto.TipoConta, "Corrente", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(dto.TipoConta, "Poupanca", StringComparison.OrdinalIgnoreCase))
            return BadRequest("TipoConta deve ser 'Corrente' ou 'Poupanca'.");

        var cliente = await _clientes.GetByIdAsync(dto.IdCliente, ct);
        if (cliente == null) return NotFound("Cliente não encontrado.");

        if (await _contas.ExistsNumeroAsync(dto.NumeroConta, null, ct))
            return Conflict("NumeroConta já cadastrado.");

        var entity = new ContaCorrente
        {
            IdCliente = dto.IdCliente,
            NumeroConta = dto.NumeroConta,
            TipoConta = char.ToUpper(dto.TipoConta[0]) + dto.TipoConta[1..].ToLower()
        };

        var created = await _contas.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.IdConta }, created);
    }

    [HttpPut("{id:int}/tipo")]
    public async Task<IActionResult> AlterarTipo(int id, ContaAlterarTipoDto dto, CancellationToken ct)
    {
        if (!string.Equals(dto.TipoConta, "Corrente", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(dto.TipoConta, "Poupanca", StringComparison.OrdinalIgnoreCase))
            return BadRequest("TipoConta deve ser 'Corrente' ou 'Poupanca'.");

        var conta = await _contas.GetByIdAsync(id, ct);
        if (conta == null) return NotFound();

        await _contas.UpdateTipoAsync(id, dto.TipoConta, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var conta = await _contas.GetByIdAsync(id, ct);
        if (conta == null) return NotFound();

        if (await _contas.HasTransacoesAsync(id, ct))
            return Conflict("Conta possui transações e não pode ser excluída.");

        await _contas.DeleteAsync(id, ct);
        return NoContent();
    }
}
