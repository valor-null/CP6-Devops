using DimDim.Api.DTOs;
using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoRepository _repo;

    public TransacoesController(ITransacaoRepository repo)
    {
        _repo = repo;
    }

    [HttpPost("deposito")]
    public async Task<ActionResult<Transacao>> Depositar(DepositoSaqueDto dto, CancellationToken ct)
    {
        try
        {
            var result = await _repo.DepositarAsync(dto.IdConta, dto.Valor, ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("saque")]
    public async Task<ActionResult<Transacao>> Sacar(DepositoSaqueDto dto, CancellationToken ct)
    {
        try
        {
            var result = await _repo.SacarAsync(dto.IdConta, dto.Valor, ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Saldo insuficiente", StringComparison.OrdinalIgnoreCase))
        {
            return Conflict(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("transferencia")]
    public async Task<ActionResult<object>> Transferir(TransferenciaDto dto, CancellationToken ct)
    {
        try
        {
            var (debito, credito) = await _repo.TransferirAsync(dto.IdContaOrigem, dto.IdContaDestino, dto.Valor, ct);
            return Ok(new { Debito = debito, Credito = credito });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Saldo insuficiente", StringComparison.OrdinalIgnoreCase))
        {
            return Conflict(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("conta/{idConta:int}")]
    public async Task<ActionResult<IEnumerable<Transacao>>> Extrato(int idConta, int take = 50, int skip = 0, CancellationToken ct = default)
    {
        var list = await _repo.ListarPorContaAsync(idConta, take, skip, ct);
        return Ok(list);
    }
}
