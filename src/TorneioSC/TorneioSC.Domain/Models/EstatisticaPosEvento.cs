namespace TorneioSC.Domain.Models
{
    public class EstatisticaPosEvento
    {
        public int EstatisticaId { get; set; }
        public int TorneioId { get; set; }
        public int MedalhasDistribuidas { get; set; }
        public int CertificadosEmitidos { get; set; }
        public int TotalLutas { get; set; }
        public DateTime GeradoEm { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Torneio Torneio { get; set; } = new Torneio();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}