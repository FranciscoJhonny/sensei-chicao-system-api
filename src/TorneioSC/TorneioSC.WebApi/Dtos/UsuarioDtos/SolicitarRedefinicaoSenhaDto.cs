using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    /// <summary>
    /// DTO para solicitação de redefinição de senha
    /// </summary>
    public class SolicitarRedefinicaoSenhaDto
    {
        /// <summary>
        /// Email do usuário para envio do token de redefinição
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]        
        public string Email { get; set; } = string.Empty;
    }
}