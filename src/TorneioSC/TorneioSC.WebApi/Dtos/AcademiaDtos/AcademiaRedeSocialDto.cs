using TorneioSC.WebApi.Dtos.RedeSocialDtos;

namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO que representa o relacionamento entre uma academia e uma rede social
    /// </summary>
    public class AcademiaRedeSocialDto
    {
        /// <summary>
        /// ID único do relacionamento academia-rede social
        /// </summary>
        public int AcademiaRedeSocialId { get; set; }

        /// <summary>
        /// ID da academia à qual a rede social está vinculada
        /// </summary>
        public int AcademiaId { get; set; }

        /// <summary>
        /// ID da rede social vinculada à academia
        /// </summary>
        public int RedeSocialId { get; set; }

        /// <summary>
        /// URL completa do perfil da academia na rede social
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
        /// Data de inclusão do relacionamento
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