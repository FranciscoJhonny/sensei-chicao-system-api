namespace TorneioSC.Domain.Models
{
    public class Resultado
    {
        public int ResultadoId { get; set; }
        public int InscricaoId { get; set; }
        public int Posicao { get; set; }
        public decimal? Pontuacao { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Inscricao Inscricao { get; set; } = new Inscricao();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}