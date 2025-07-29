namespace TorneioSC.Domain.Models
{
    public class EventoTorneio
    {
        public int EventoTorneioId { get; set; }
        public int EventoId { get; set; }
        public int TorneioId { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Evento Evento { get; set; } = new Evento();
        public Torneio Torneio { get; set; } = new Torneio();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}