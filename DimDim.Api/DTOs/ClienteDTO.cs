namespace DimDim.Api.DTOs;

public class ClienteDto
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
}