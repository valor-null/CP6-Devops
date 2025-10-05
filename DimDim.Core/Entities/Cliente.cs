namespace DimDim.Core.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<ContaCorrente>? Contas { get; set; }
    }
}