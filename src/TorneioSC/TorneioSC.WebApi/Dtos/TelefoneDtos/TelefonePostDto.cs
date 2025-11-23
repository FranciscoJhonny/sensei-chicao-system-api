using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.TelefoneDtos
{
    /// <summary>
    /// DTO para criação de um novo telefone
    /// </summary>
    public class TelefonePostDto
    {
        /// <summary>
        /// Número do telefone (formato: (XX) XXXXX-XXXX ou variações)
        /// </summary>
        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [StringLength(15, ErrorMessage = "O número de telefone não pode exceder 15 caracteres.")]
        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string NumeroTelefone { get; set; } = string.Empty;

        /// <summary>
        /// ID do tipo de telefone (ex: 1=Celular, 2=Residencial, 3=Comercial)
        /// </summary>
        [Required(ErrorMessage = "O tipo de telefone é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Tipo de telefone inválido.")]
        public int TipoTelefoneId { get; set; }

        /// <summary>
        /// ID do usuário que está criando o telefone
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}