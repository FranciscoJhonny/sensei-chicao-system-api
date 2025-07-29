namespace TorneioSC.Domain.Models
{
    public class UsuarioLogadoVM
    {

        /// <summary>
        /// Usuario logado no sistema
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nome do Usuario Logado no sistema
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Perfil do usuário
        /// </summary>
        public int PerfilId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DescricaoPerfil { get; set; } = string.Empty;
        public Perfil Perfil { get; set; } = new Perfil();

    }
}
