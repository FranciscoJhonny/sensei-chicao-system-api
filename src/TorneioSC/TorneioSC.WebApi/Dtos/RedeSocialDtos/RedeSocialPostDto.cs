using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    /// <summary>
    /// DTO para criação de um vínculo com rede social
    /// </summary>
    public class RedeSocialPostDto
    {
        /// <summary>
        /// ID do tipo de rede social (ex: 1=Facebook, 2=Instagram, 3=Twitter, etc.)
        /// </summary>
        [Required(ErrorMessage = "O ID da rede social é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da rede social inválido.")]
        public int RedeSocialId { get; set; }

        /// <summary>
        /// URL completa do perfil na rede social
        /// </summary>
        [Required(ErrorMessage = "A URL do perfil é obrigatória.")]
        [StringLength(500, ErrorMessage = "A URL do perfil não pode exceder 500 caracteres.")]
        [Url(ErrorMessage = "A URL do perfil deve ser válida.")]
        public string PerfilUrl { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que está criando o vínculo com a rede social
        /// </summary>
        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}