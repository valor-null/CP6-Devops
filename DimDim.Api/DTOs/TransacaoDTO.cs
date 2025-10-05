using System.ComponentModel.DataAnnotations;

namespace DimDim.Api.DTOs;

public class DepositoSaqueDto
{
    [Range(1, int.MaxValue)]
    public int IdConta { get; set; }

    [Required]
    public decimal Valor { get; set; }
}

public class TransferenciaDto
{
    [Range(1, int.MaxValue)]
    public int IdContaOrigem { get; set; }

    [Range(1, int.MaxValue)]
    public int IdContaDestino { get; set; }

    [Required]
    public decimal Valor { get; set; }
}