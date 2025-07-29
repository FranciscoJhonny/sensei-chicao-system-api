namespace TorneioSC.Domain.Models
{
    public class Perfil
    {
        public int PerfilId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        //public Usuario UsuarioInclusao { get; set; } = new Usuario();
        //public Usuario UsuarioOperacao { get; set; } = new Usuario();
        public ICollection<Usuario> Usuarios { get; set; } = new  List<Usuario>();
    }
}