namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    /// <summary>
    /// DTO para redefinição de senha do usuário
    /// </summary>
    public class RedefinirSenhaDto
    {
        /// <summary>
        /// Token de redefinição de senha recebido por email
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nova senha do usuário
        /// </summary>
        public string NovaSenha { get; set; } = string.Empty;
    }
}