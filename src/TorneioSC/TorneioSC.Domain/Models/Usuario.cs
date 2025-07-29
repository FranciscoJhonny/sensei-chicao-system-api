namespace TorneioSC.Domain.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public int PerfilId { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }
        public DateTime? DataRecuperacaoSenha { get; set; }

        public Perfil Perfil { get; set; } = new Perfil();
        //public Usuario? UsuarioInclusao { get; set; }
        //public Usuario? UsuarioOperacao { get; set; }
    }
}