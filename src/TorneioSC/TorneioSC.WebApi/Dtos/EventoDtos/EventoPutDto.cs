using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.TorneioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO para atualização de um evento existente
    /// </summary>
    public class EventoPutDto
    {
        /// <summary>
        /// ID único do evento a ser atualizado
        /// </summary>
        [Required(ErrorMessage = "O ID do evento é obrigatório para atualização.")]
        public int EventoId { get; set; }

        /// <summary>
        /// Nome do evento
        /// </summary>
        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome do evento não pode exceder 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de início do evento
        /// </summary>
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término do evento
        /// </summary>
        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Local onde o evento será realizado
        /// </summary>
        [Required(ErrorMessage = "O local é obrigatório.")]
        [StringLength(200, ErrorMessage = "O local não pode exceder 200 caracteres.")]
        public string Local { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pelo evento
        /// </summary>
        [Required(ErrorMessage = "O responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do responsável não pode exceder 100 caracteres.")]
        public string Responsavel { get; set; } = string.Empty;

        /// <summary>
        /// Email do responsável pelo evento
        /// </summary>
        [Required(ErrorMessage = "O email do responsável é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email do responsável inválido.")]
        public string EmailResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Telefone do responsável pelo evento
        /// </summary>
        [Required(ErrorMessage = "O telefone do responsável é obrigatório.")]
        [Phone(ErrorMessage = "Telefone do responsável inválido.")]
        public string TelefoneResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Observações adicionais sobre o evento (opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "As observações não podem exceder 500 caracteres.")]
        public string? Observacoes { get; set; }

        /// <summary>
        /// Indica se o evento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que está realizando a atualização
        /// </summary>
        public int UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Dados do torneio associado ao evento para atualização
        /// </summary>
        public TorneioPutDto Torneio { get; set; } = null!;
    }
}