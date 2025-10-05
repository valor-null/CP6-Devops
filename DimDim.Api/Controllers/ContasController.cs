using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DimDim.Infrastructure.Data;
using DimDim.Core.Entities;

namespace DimDim.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly DimDimContext _context;

    public ContasController(DimDimContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContaCorrente>>> GetAll()
    {
        return await _context.ContasCorrente.Include(c => c.Cliente).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaCorrente>> GetById(int id)
    {
        var conta = await _context.ContasCorrente.Include(c => c.Cliente).FirstOrDefaultAsync(c => c.IdConta == id);
        if (conta == null) return NotFound();
        return conta;
    }

    [HttpPost]
    public async Task<ActionResult<ContaCorrente>> Create(ContaCorrente conta)
    {
        _context.ContasCorrente.Add(conta);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = conta.IdConta }, conta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ContaCorrente conta)
    {
        if (id != conta.IdConta) return BadRequest();
        _context.Entry(conta).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var conta = await _context.ContasCorrente.FindAsync(id);
        if (conta == null) return NotFound();
        _context.ContasCorrente.Remove(conta);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}