namespace DimDim.Core.Entities;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = null!;
    public string CPF { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DataCadastro { get; set; }
    public ICollection<ContaCorrente> Contas { get; set; } = new List<ContaCorrente>();
}