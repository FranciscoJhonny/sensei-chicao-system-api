namespace TorneioSC.WebApi.Dtos.TokenResponseDtos
{
    /// <summary>
    /// DTO que representa a resposta de autenticação com token JWT
    /// </summary>
    public class TokenResponseDto
    {
        /// <summary>
        /// Token de acesso JWT para autenticação nas requisições
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do token (padrão: "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Tempo de expiração do token em segundos
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}