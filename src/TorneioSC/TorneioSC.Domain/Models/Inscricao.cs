namespace TorneioSC.Domain.Models
{
    public class Inscricao
    {
        public int InscricaoId { get; set; }
        public int AtletaId { get; set; }
        public int TorneioId { get; set; }
        public int CategoriaId { get; set; }
        public int ModalidadeId { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Atleta Atleta { get; set; } = new Atleta();
        public Torneio Torneio { get; set; } = new Torneio();
        public Categoria Categoria { get; set; } = new Categoria();
        public Modalidade Modalidade { get; set; } = new Modalidade();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
        public ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();
        public ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();
    }
}