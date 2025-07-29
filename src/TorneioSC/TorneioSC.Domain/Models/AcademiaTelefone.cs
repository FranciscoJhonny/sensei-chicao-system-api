namespace TorneioSC.Domain.Models
{
    public class AcademiaTelefone
    {
        public int AcademiaTelefoneId { get; set; }
        public int TelefoneId { get; set; }
        public int AcademiaId { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public Academia Academia { get; set; } = new Academia();
        public Telefone Telefone { get; set; } = new Telefone();
        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
    }
}