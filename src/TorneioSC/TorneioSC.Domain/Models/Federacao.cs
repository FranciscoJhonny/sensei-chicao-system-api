namespace TorneioSC.Domain.Models
{
    public class Federacao
    {
        public int FederacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Site { get; set; } = string.Empty;
        public DateTime DataFundacao { get; set; }
        public string Portaria { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Municipio Municipio { get; set; } = new Municipio();
        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
        public ICollection<Academia> Academias { get; set; } = new List<Academia>();
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Telefone> Telefones { get; set; } = new List<Telefone>();
        public ICollection<FederacaoRedeSocial> FederacaoRedeSociais { get; set; } = new List<FederacaoRedeSocial>();
    }
}