using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DimDim.Infrastructure.Data;
using DimDim.Core.Entities;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly DimDimContext _context;

    public TransacoesController(DimDimContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetAll()
    {
        return await _context.Transacoes.Include(t => t.Conta).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transacao>> GetById(int id)
    {
        var transacao = await _context.Transacoes.Include(t => t.Conta).FirstOrDefaultAsync(t => t.IdTransacao == id);
        if (transacao == null) return NotFound();
        return transacao;
    }

    [HttpPost]
    public async Task<ActionResult<Transacao>> Create(Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = transacao.IdTransacao }, transacao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Transacao transacao)
    {
        if (id != transacao.IdTransacao) return BadRequest();
        _context.Entry(transacao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null) return NotFound();
        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}