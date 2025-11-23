using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    /// <summary>
    /// DTO para criação de um novo usuário
    /// </summary>
    public class UsuarioPostDto
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário (utilizado para login)
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário (mínimo 6 caracteres)
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>
        /// ID do perfil do usuário
        /// </summary>
        [Required(ErrorMessage = "O perfil é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O perfil informado não é válido.")]
        public int PerfilId { get; set; }
    }
}