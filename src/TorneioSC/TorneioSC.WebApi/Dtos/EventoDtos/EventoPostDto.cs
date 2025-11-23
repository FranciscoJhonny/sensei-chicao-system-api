using TorneioSC.WebApi.Dtos.TorneioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO para criação de um novo evento
    /// </summary>
    public class EventoPostDto
    {
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
        /// Email do responsável pelo evento
        /// </summary>
        public string EmailResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Telefone do responsável pelo evento
        /// </summary>
        public string TelefoneResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Observações adicionais sobre o evento
        /// </summary>
        public string Observacoes { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o evento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que está criando o evento
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Dados do torneio associado ao evento
        /// </summary>
        public TorneioPostDto Torneio { get; set; } = new TorneioPostDto();
    }
}