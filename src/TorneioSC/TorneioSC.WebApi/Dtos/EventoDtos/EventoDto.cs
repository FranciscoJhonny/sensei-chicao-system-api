using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO que representa um evento no sistema
    /// </summary>
    public class EventoDto
    {
        /// <summary>
        /// ID único do evento
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Nome do evento
        /// </summary>
        public string NomeEvento { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de início do evento
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término do evento
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Local onde o evento será realizado
        /// </summary>
        public string Local { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pelo evento
        /// </summary>
        public string Responsavel { get; set; } = string.Empty;

        /// <summary>
        /// Email do responsável pelo evento (opcional)
        /// </summary>
        public string? EmailResponsavel { get; set; }

        /// <summary>
        /// Telefone do responsável pelo evento (opcional)
        /// </summary>
        public string? TelefoneResponsavel { get; set; }

        /// <summary>
        /// Observações adicionais sobre o evento (opcional)
        /// </summary>
        public string? Observacoes { get; set; }

        /// <summary>
        /// Indica se o evento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que criou o evento
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do evento no sistema
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
        /// Usuário que criou o evento
        /// </summary>
        public UsuarioDto? UsuarioInclusao { get; set; }

        /// <summary>
        /// Usuário que realizou a última operação no evento
        /// </summary>
        public UsuarioDto? UsuarioOperacao { get; set; }

        /// <summary>
        /// Torneio associado ao evento
        /// </summary>
        public Torneio Torneio { get; set; } = new Torneio();
    }
}