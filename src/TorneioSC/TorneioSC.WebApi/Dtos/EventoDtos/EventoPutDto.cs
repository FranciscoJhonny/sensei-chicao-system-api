using System.ComponentModel.DataAnnotations;
using TorneioSC.WebApi.Dtos.TorneioDtos;

namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO para atualização de um evento existente.
    /// </summary>
    public class EventoPutDto
    {
        [Required(ErrorMessage = "O ID do evento é obrigatório para atualização.")]
        public int EventoId { get; set; }

        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome do evento não pode exceder 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória.")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O local é obrigatório.")]
        [StringLength(200, ErrorMessage = "O local não pode exceder 200 caracteres.")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "O responsável é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do responsável não pode exceder 100 caracteres.")]
        public string Responsavel { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email do responsável é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email do responsável inválido.")]
        public string EmailResponsavel { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone do responsável é obrigatório.")]
        [Phone(ErrorMessage = "Telefone do responsável inválido.")]
        public string TelefoneResponsavel { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "As observações não podem exceder 500 caracteres.")]
        public string? Observacoes { get; set; }

        public bool Ativo { get; set; }

        public int UsuarioOperacaoId { get; set; }

        // Torneio
        public TorneioPutDto Torneio { get; set; } = null!;
    }
}