using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Local { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string? EmailResponsavel { get; set; }
        public string? TelefoneResponsavel { get; set; }
        public string? Observacoes { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
        public Torneio Torneio { get; set; } = new Torneio();
    }
}
