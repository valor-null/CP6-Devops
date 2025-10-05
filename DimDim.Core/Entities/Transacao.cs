namespace DimDim.Core.Entities
{
    public class Transacao
    {
        public int IdTransacao { get; set; }
        public int IdConta { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;

        public ContaCorrente? Conta { get; set; }
    }
}