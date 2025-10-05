using System.ComponentModel.DataAnnotations;

namespace DimDim.Api.DTOs;

public class ClienteCreateDto
{
    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\d{11}$")]
    public string CPF { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;
}

public class ClienteUpdateDto
{
    [Required]
    [StringLength(150)]
    public string Nome { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;
}