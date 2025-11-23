using TorneioSC.WebApi.Dtos.PerfilDtos;

namespace TorneioSC.WebApi.Dtos.UsuarioDtos
{
    /// <summary>
    /// DTO que representa um usuário no sistema
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// ID único do usuário
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário (utilizado para login)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha do usuário
        /// </summary>
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>
        /// ID do perfil do usuário
        /// </summary>
        public int PerfilId { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que criou este usuário
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do usuário no sistema
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Natureza da operação (I=Inclusão, A=Alteração, E=Exclusão)
        /// </summary>
        public string? NaturezaOperacao { get; set; }

        /// <summary>
        /// ID do usuário que realizou a última operação
        /// </summary>
        public int? UsuarioOperacaoId { get; set; }

        /// <summary>
        /// Data da última operação realizada
        /// </summary>
        public DateTime? DataOperacao { get; set; }

        /// <summary>
        /// Dados completos do perfil do usuário
        /// </summary>
        public PerfilDto Perfil { get; set; } = new PerfilDto();
    }
}