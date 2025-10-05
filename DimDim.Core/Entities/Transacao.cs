namespace DimDim.Core.Entities;

public class Transacao
{
    public int IdTransacao { get; set; }
    public int IdConta { get; set; }
    public string Tipo { get; set; } = null!;
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; }
    public ContaCorrente Conta { get; set; } = null!;
}