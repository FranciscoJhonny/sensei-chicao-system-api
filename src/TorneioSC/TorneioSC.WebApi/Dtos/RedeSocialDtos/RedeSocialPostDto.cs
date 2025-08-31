using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    public class RedeSocialPostDto
    {
        [Required(ErrorMessage = "O ID da rede social é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da rede social inválido.")]
        public int RedeSocialId { get; set; }

        [Required(ErrorMessage = "A URL do perfil é obrigatória.")]
        [StringLength(500, ErrorMessage = "A URL do perfil não pode exceder 500 caracteres.")]
        [Url(ErrorMessage = "A URL do perfil deve ser válida.")]
        public string PerfilUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID do usuário que está incluindo é obrigatório.")]
        public int UsuarioInclusaoId { get; set; }
    }
}