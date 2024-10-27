namespace PariocaFight.VO
{
    public class PagamentoVO
    {
        public int PagamentoId { get; set; }
        public int AlunoId { get; set; }
        public DateTime DataPagamento { get; set; }
        public Decimal Valor { get; set; }
        public string Status { get; set; }
        public string FormaPagamento { get; set; }
        public string Nome { get; set; }


    }
}
