using System.ComponentModel.DataAnnotations;

namespace PariocaFight.VO
{
    public class AlunosVO
    {
        [Key]
        public int AlunoId { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public int Idade { get; set; }
        public string? Faixa { get; set; }
        public string? Contato { get; set; }
        public string? NomeResponsavel { get; set; }
    }
}
