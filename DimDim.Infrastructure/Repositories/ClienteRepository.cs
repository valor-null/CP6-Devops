using System.Linq.Expressions;
using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using DimDim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly DimDimContext _ctx;

    public ClienteRepository(DimDimContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken ct = default)
    {
        return await _ctx.Clientes.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _ctx.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.IdCliente == id, ct);
    }

    public async Task<Cliente> CreateAsync(Cliente entity, CancellationToken ct = default)
    {
        _ctx.Clientes.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Cliente entity, CancellationToken ct = default)
    {
        _ctx.Clientes.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id, ct);
        if (entity == null) return;
        _ctx.Clientes.Remove(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsCpfAsync(string cpf, int? ignoreId = null, CancellationToken ct = default)
    {
        return await _ctx.Clientes.AnyAsync(c => c.CPF == cpf && (ignoreId == null || c.IdCliente != ignoreId.Value), ct);
    }

    public async Task<bool> ExistsEmailAsync(string email, int? ignoreId = null, CancellationToken ct = default)
    {
        return await _ctx.Clientes.AnyAsync(c => c.Email == email && (ignoreId == null || c.IdCliente != ignoreId.Value), ct);
    }

    public async Task<bool> HasContasAsync(int idCliente, CancellationToken ct = default)
    {
        return await _ctx.Contas.AnyAsync(c => c.IdCliente == idCliente, ct);
    }
}
