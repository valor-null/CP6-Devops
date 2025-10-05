using System.ComponentModel.DataAnnotations;

namespace DimDim.Api.DTOs;

public class DepositoSaqueDto
{
    [Range(1, int.MaxValue)]
    public int IdConta { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Valor { get; set; }
}

public class TransferenciaDto
{
    [Range(1, int.MaxValue)]
    public int IdContaOrigem { get; set; }

    [Range(1, int.MaxValue)]
    public int IdContaDestino { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Valor { get; set; }
}