using TorneioSC.WebApi.Dtos.EventoDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EventoTorneioDtos
{
    /// <summary>
    /// DTO que representa o relacionamento entre um evento e um torneio
    /// </summary>
    public class EventoTorneioDto
    {
        /// <summary>
        /// ID único do relacionamento evento-torneio
        /// </summary>
        public int EventoTorneioId { get; set; }

        /// <summary>
        /// ID do evento ao qual o torneio está vinculado
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// ID do torneio vinculado ao evento
        /// </summary>
        public int TorneioId { get; set; }

        /// <summary>
        /// Indica se o relacionamento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que criou o relacionamento
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do relacionamento no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Dados completos do evento vinculado
        /// </summary>
        public EventoDto Evento { get; set; } = new EventoDto();

        /// <summary>
        /// Dados completos do torneio vinculado
        /// </summary>
        public TorneioDto Torneio { get; set; } = new TorneioDto();

        /// <summary>
        /// Usuário que criou o relacionamento
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no relacionamento
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}