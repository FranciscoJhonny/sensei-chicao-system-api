using TorneioSC.WebApi.Dtos.RedeSocialDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO que representa o relacionamento entre uma federação e uma rede social
    /// </summary>
    public class FederacaoRedeSocialDto
    {
        /// <summary>
        /// ID único do relacionamento federação-rede social
        /// </summary>
        public int FederacaoRedeSocialId { get; set; }

        /// <summary>
        /// ID da federação à qual a rede social está vinculada
        /// </summary>
        public int FederacaoId { get; set; }

        /// <summary>
        /// ID da rede social vinculada à federação
        /// </summary>
        public int RedeSocialId { get; set; }

        /// <summary>
        /// URL completa do perfil da federação na rede social
        /// </summary>
        public string PerfilUrl { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o perfil na rede social está ativo
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que incluiu o relacionamento
        /// </summary>
        public int UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de inclusão do relacionamento no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string NaturezaOperacao { get; set; } = string.Empty;

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime DataOperacao { get; set; }

        /// <summary>
        /// Dados da rede social vinculada
        /// </summary>
        public RedeSocialDto RedeSocial { get; set; } = new RedeSocialDto();
    }
}