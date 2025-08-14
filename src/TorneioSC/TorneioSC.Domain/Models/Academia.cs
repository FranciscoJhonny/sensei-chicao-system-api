namespace TorneioSC.Domain.Models
{
    public class Academia
    {
        public int AcademiaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? FederacaoId { get; set; }
        public int MunicipioId { get; set; }
        public bool Ativo { get; set; } = true;
        public int UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string NaturezaOperacao { get; set; } = string.Empty;
        public int UsuarioOperacaoId { get; set; }
        public DateTime DataOperacao { get; set; }

        public Federacao? Federacao { get; set; }
        public Municipio Municipio { get; set; } = new Municipio();
        public Usuario UsuarioInclusao { get; set; } = new Usuario();
        public Usuario UsuarioOperacao { get; set; } = new Usuario();
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Telefone> Telefones { get; set; } = new List<Telefone>();
        public ICollection<Atleta> Atletas { get; set; } = new List<Atleta>();
        public ICollection<Torneio> Torneios { get; set; } = new List<Torneio>();
        public ICollection<EquipePontuacao> Pontuacoes { get; set; } = new List<EquipePontuacao>();
    }
}