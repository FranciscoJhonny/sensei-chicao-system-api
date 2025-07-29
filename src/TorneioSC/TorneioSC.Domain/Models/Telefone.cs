namespace TorneioSC.Domain.Models
{
    public class Telefone
    {
        public int TelefoneId { get; set; }
        public string? NumeroTelefone { get; set; }
        public int? TipoTelefoneId { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public TipoTelefone? TipoTelefone { get; set; }
        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
        public ICollection<AcademiaTelefone> Academias { get; set; } = new List<AcademiaTelefone>();
        public ICollection<FederacaoTelefone> Federacoes { get; set; } = new List<FederacaoTelefone>();
    }
}