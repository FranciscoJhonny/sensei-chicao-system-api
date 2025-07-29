namespace TorneioSC.SqlServerAdapter.Entities
{

    public partial class Usuario
    {
        public int UsuarioID { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string SenhaHash { get; set; } = null!;

        public string Perfil { get; set; } = null!;

        public bool Ativo { get; set; }

        public int? UsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }

        public string? NaturezaOperacao { get; set; }

        public int? UsuarioOperacao { get; set; }

        public DateTime? DataOperacao { get; set; }
    }
}