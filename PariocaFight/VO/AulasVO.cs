namespace PariocaFight.VO
{
    public class AulasVO
    {
        public int Aulaid { get; set; }
        public TimeSpan Horario { get; set; }
        public string? DiaSemana { get; set; }
        public int InstrutorId { get; set; }
        public string? NomeAula { get; set; }
        public string? HorarioFormatado { get; set; }
        public string? Nome { get; set; }

        public List<InstrutoresVO> instrutoresVO { get; set; }

    }
}
