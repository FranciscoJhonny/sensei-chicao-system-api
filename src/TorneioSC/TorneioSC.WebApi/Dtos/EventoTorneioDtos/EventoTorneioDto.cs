using TorneioSC.WebApi.Dtos.EventoDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EventoTorneioDtos
{
    public class EventoTorneioDto
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

        public EventoDto Evento { get; set; } = new EventoDto();
        public TorneioDto Torneio { get; set; } = new TorneioDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
