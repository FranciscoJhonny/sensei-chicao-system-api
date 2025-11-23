namespace TorneioSC.WebApi.Dtos.RedeSocialDtos
{
    /// <summary>
    /// DTO para atualização de um vínculo com rede social
    /// </summary>
    public class RedeSocialPutDto
    {
        /// <summary>
        /// ID do relacionamento federação-rede social (usado quando atualizando rede social de federação)
        /// </summary>
        public int FederacaoRedeSocialId { get; set; }

        /// <summary>
        /// ID do relacionamento academia-rede social (usado quando atualizando rede social de academia)
        /// </summary>
        public int AcademiaRedeSocialId { get; set; }

        /// <summary>
        /// ID do tipo de rede social (ex: 1=Facebook, 2=Instagram, 3=Twitter, etc.)
        /// </summary>
        public int RedeSocialId { get; set; }

        /// <summary>
        /// URL completa do perfil na rede social
        /// </summary>
        public string PerfilUrl { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o vínculo com a rede social está ativo
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// ID do usuário que está realizando a atualização
        /// </summary>
        public int UsuarioAlteracaoId { get; set; }
    }
}