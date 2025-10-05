using DimDim.Core.Entities;

namespace DimDim.Core.Interfaces;

public interface IClienteRepository
{
    Task<IReadOnlyList<Cliente>> GetAllAsync(CancellationToken ct = default);
    Task<Cliente?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Cliente> CreateAsync(Cliente entity, CancellationToken ct = default);
    Task UpdateAsync(Cliente entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsCpfAsync(string cpf, int? ignoreId = null, CancellationToken ct = default);
    Task<bool> ExistsEmailAsync(string email, int? ignoreId = null, CancellationToken ct = default);
    Task<bool> HasContasAsync(int idCliente, CancellationToken ct = default);
}