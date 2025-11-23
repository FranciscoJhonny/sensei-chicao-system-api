namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    /// <summary>
    /// DTO que representa uma rede social no sistema
    /// </summary>
    public class RedeSocialDto
    {
        /// <summary>
        /// ID único da rede social
        /// </summary>
        public int RedeSocialId { get; set; }

        /// <summary>
        /// URL completa do perfil na rede social
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que incluiu a rede social (opcional)
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }
    }
}