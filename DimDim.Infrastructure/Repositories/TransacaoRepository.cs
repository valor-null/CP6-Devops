using DimDim.Core.Entities;
using DimDim.Core.Interfaces;
using DimDim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly DimDimContext _ctx;

    public TransacaoRepository(DimDimContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Transacao> DepositarAsync(int idConta, decimal valor, CancellationToken ct = default)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
        var conta = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta, ct);
        if (conta == null) throw new InvalidOperationException("Conta não encontrada.");
        await using var trx = await _ctx.Database.BeginTransactionAsync(ct);
        try
        {
            conta.Saldo += valor;
            var transacao = new Transacao { IdConta = idConta, Tipo = "CREDITO", Valor = valor };
            _ctx.Transacoes.Add(transacao);
            await _ctx.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
            return transacao;
        }
        catch (DbUpdateConcurrencyException)
        {
            await trx.RollbackAsync(ct);
            throw new InvalidOperationException("Conflito de concorrência ao atualizar saldo.");
        }
    }

    public async Task<Transacao> SacarAsync(int idConta, decimal valor, CancellationToken ct = default)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
        var conta = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == idConta, ct);
        if (conta == null) throw new InvalidOperationException("Conta não encontrada.");
        if (conta.Saldo < valor) throw new InvalidOperationException("Saldo insuficiente.");
        await using var trx = await _ctx.Database.BeginTransactionAsync(ct);
        try
        {
            conta.Saldo -= valor;
            var transacao = new Transacao { IdConta = idConta, Tipo = "DEBITO", Valor = valor };
            _ctx.Transacoes.Add(transacao);
            await _ctx.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
            return transacao;
        }
        catch (DbUpdateConcurrencyException)
        {
            await trx.RollbackAsync(ct);
            throw new InvalidOperationException("Conflito de concorrência ao atualizar saldo.");
        }
    }

    public async Task<(Transacao Debito, Transacao Credito)> TransferirAsync(int idContaOrigem, int idContaDestino, decimal valor, CancellationToken ct = default)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
        if (idContaOrigem == idContaDestino) throw new ArgumentException("Contas de origem e destino devem ser diferentes.");
        var origem = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaOrigem, ct);
        var destino = await _ctx.Contas.FirstOrDefaultAsync(c => c.IdConta == idContaDestino, ct);
        if (origem == null) throw new InvalidOperationException("Conta de origem não encontrada.");
        if (destino == null) throw new InvalidOperationException("Conta de destino não encontrada.");
        if (origem.Saldo < valor) throw new InvalidOperationException("Saldo insuficiente na conta de origem.");
        await using var trx = await _ctx.Database.BeginTransactionAsync(ct);
        try
        {
            origem.Saldo -= valor;
            destino.Saldo += valor;
            var debito = new Transacao { IdConta = idContaOrigem, Tipo = "DEBITO", Valor = valor };
            var credito = new Transacao { IdConta = idContaDestino, Tipo = "CREDITO", Valor = valor };
            _ctx.Transacoes.AddRange(debito, credito);
            await _ctx.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);
            return (debito, credito);
        }
        catch (DbUpdateConcurrencyException)
        {
            await trx.RollbackAsync(ct);
            throw new InvalidOperationException("Conflito de concorrência ao atualizar saldo.");
        }
    }

    public async Task<IReadOnlyList<Transacao>> ListarPorContaAsync(int idConta, int take, int skip = 0, CancellationToken ct = default)
    {
        return await _ctx.Transacoes
            .Where(t => t.IdConta == idConta)
            .OrderByDescending(t => t.DataHora)
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
