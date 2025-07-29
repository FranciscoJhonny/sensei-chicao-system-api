using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    public class UsuarioPutDto
    {
        public int UsuarioId { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string SenhaHash { get; set; } = string.Empty;

        [Required, Range(1, int.MaxValue)]
        public int PerfilId { get; set; }
    }
}
