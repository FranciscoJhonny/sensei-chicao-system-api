using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.PerfilDtos
{
    /// <summary>
    /// DTO que representa um perfil de usuário no sistema
    /// </summary>
    public class PerfilDto
    {
        /// <summary>
        /// ID único do perfil
        /// </summary>
        public int PerfilId { get; set; }

        /// <summary>
        /// Descrição/nome do perfil (ex: "Administrador", "Organizador", "Árbitro", "Atleta")
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o perfil está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// ID do usuário que criou o perfil
        /// </summary>
        public int? UsuarioInclusaoId { get; set; }

        /// <summary>
        /// Data de criação do perfil no sistema
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
        /// Lista de usuários que possuem este perfil
        /// </summary>
        public ICollection<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();
    }
}