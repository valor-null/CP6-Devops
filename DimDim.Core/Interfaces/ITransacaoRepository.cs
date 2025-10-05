using DimDim.Core.Entities;

namespace DimDim.Core.Interfaces;

public interface ITransacaoRepository
{
    Task<Transacao> DepositarAsync(int idConta, decimal valor, CancellationToken ct = default);
    Task<Transacao> SacarAsync(int idConta, decimal valor, CancellationToken ct = default);
    Task<(Transacao Debito, Transacao Credito)> TransferirAsync(int idContaOrigem, int idContaDestino, decimal valor, CancellationToken ct = default);
    Task<IReadOnlyList<Transacao>> ListarPorContaAsync(int idConta, int take, int skip = 0, CancellationToken ct = default);
}