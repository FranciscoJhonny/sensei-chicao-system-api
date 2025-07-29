namespace TorneioSC.Domain.Models
{
    public class FederacaoEndereco
    {
        public int FederacaoEnderecoId { get; set; }
        public int FederacaoId { get; set; }
        public int EnderecoId { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public Federacao Federacao { get; set; } = new Federacao();
        public Endereco Endereco { get; set; } = new Endereco();
        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
    }
}