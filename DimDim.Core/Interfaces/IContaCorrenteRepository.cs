using DimDim.Core.Entities;

namespace DimDim.Core.Interfaces;

public interface IContaCorrenteRepository
{
    Task<IReadOnlyList<ContaCorrente>> GetAllAsync(CancellationToken ct = default);
    Task<ContaCorrente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ContaCorrente?> GetByNumeroAsync(string numeroConta, CancellationToken ct = default);
    Task<ContaCorrente> CreateAsync(ContaCorrente entity, CancellationToken ct = default);
    Task UpdateTipoAsync(int id, string novoTipo, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsNumeroAsync(string numeroConta, int? ignoreId = null, CancellationToken ct = default);
    Task<bool> HasTransacoesAsync(int idConta, CancellationToken ct = default);
}