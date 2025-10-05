namespace DimDim.Core.Entities;

public class ContaCorrente
{
    public int IdConta { get; set; }
    public string NumeroConta { get; set; } = null!;
    public decimal Saldo { get; set; }
    public string TipoConta { get; set; } = null!;
    public int IdCliente { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    public byte[]? RowVersion { get; set; }
}