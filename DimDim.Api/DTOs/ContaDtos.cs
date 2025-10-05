using System.ComponentModel.DataAnnotations;

namespace DimDim.Api.DTOs;

public class ContaCreateDto
{
    [Range(1, int.MaxValue)]
    public int IdCliente { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string NumeroConta { get; set; } = null!;

    [Required]
    [RegularExpression("^(Corrente|Poupanca)$", ErrorMessage = "TipoConta deve ser 'Corrente' ou 'Poupanca'.")]
    public string TipoConta { get; set; } = null!;
}

public class ContaAlterarTipoDto
{
    [Required]
    [RegularExpression("^(Corrente|Poupanca)$", ErrorMessage = "TipoConta deve ser 'Corrente' ou 'Poupanca'.")]
    public string TipoConta { get; set; } = null!;
}