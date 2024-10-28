using System.ComponentModel.DataAnnotations;

namespace PariocaFight.VO
{
    public class InstrutoresVO
    {
        [Key]
        public int instrutorId { get; set; }
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Faixa { get; set; }
        public string? ArteMarcial { get; set; }
    }
}
