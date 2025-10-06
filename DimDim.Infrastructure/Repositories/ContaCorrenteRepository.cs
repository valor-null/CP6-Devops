using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using DimDim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DimDim.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly DimDimContext _ctx;

    public ContaCorrenteRepository(DimDimContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IReadOnlyList<ContaCorrente>> GetAllAsync(CancellationToken ct = default)
    {
        return await _ctx.Contas.AsNoTracking().ToListAsync(ct);
    }

    public async Task<ContaCorrente?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _ctx.Contas.AsNoTracking().FirstOrDefaultAsync(c => c.IdConta == id, ct);
    }

    public async Task<ContaCorrente?> GetByNumeroAsync(string numeroConta, CancellationToken ct = default)
    {
        return await _ctx.Contas.AsNoTracking().FirstOrDefaultAsync(c => c.NumeroConta == numeroConta, ct);
    }

    public async Task<ContaCorrente> CreateAsync(ContaCorrente entity, CancellationToken ct = default)
    {
        _ctx.Contas.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateTipoAsync(int id, string novoTipo, CancellationToken ct = default)
    {
        var entity = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == id, ct);
        if (entity is null) return;
        entity.TipoConta = novoTipo;
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var conta = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == id, ct);
        if (conta is null) return;

        var trans = await _ctx.Transacoes.Where(t => t.IdConta == id).ToListAsync(ct);
        if (trans.Count > 0) _ctx.Transacoes.RemoveRange(trans);

        _ctx.Contas.Remove(conta);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsNumeroAsync(string numeroConta, int? ignoreId = null, CancellationToken ct = default)
    {
        return await _ctx.Contas.AnyAsync(c => c.NumeroConta == numeroConta && (ignoreId == null || c.IdConta != ignoreId.Value), ct);
    }

    public async Task<bool> HasTransacoesAsync(int idConta, CancellationToken ct = default)
    {
        return await _ctx.Transacoes.AnyAsync(t => t.IdConta == idConta, ct);
    }
}
