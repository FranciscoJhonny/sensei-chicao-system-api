using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    public class SolicitarRedefinicaoSenhaDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
