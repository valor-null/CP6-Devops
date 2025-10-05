namespace DimDim.Core.Entities
{
    public class ContaCorrente
    {
        public int IdConta { get; set; }
        public string NumeroConta { get; set; } = string.Empty;
        public decimal Saldo { get; set; } = 0;
        public string TipoConta { get; set; } = "Corrente";
        public int IdCliente { get; set; }

        public Cliente? Cliente { get; set; }
        public ICollection<Transacao>? Transacoes { get; set; }
    }
}