namespace TorneioSC.Domain.Models
{
    public class Endereco
    {
        public int EnderecoId { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
        public ICollection<AcademiaEndereco> Academias { get; set; } = new List<AcademiaEndereco>();
        public ICollection<FederacaoEndereco> Federacoes { get; set; } = new List<FederacaoEndereco>();
    }
}