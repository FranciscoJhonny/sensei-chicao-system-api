namespace TorneioSC.Domain.Models
{
    public class Chaveamento
    {
        public int ChaveamentoId { get; set; }
        public int TorneioId { get; set; }
        public int CategoriaId { get; set; }
        public int ModalidadeId { get; set; }
        public string? DadosChave { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Torneio Torneio { get; set; } = new Torneio();
        public Categoria Categoria { get; set; } = new Categoria();
        public Modalidade Modalidade { get; set; } = new Modalidade();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
    }
}