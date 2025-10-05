namespace DimDim.Api.DTOs;

public class TransacaoDto
{
    public int IdTransacao { get; set; }
    public int IdConta { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; }
}