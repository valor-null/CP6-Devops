namespace DimDim.Api.DTOs;

public class ContaCorrenteDto
{
    public int IdConta { get; set; }
    public string NumeroConta { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public string TipoConta { get; set; } = string.Empty;
    public int IdCliente { get; set; }
}