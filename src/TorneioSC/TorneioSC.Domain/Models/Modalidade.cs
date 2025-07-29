namespace TorneioSC.Domain.Models
{
    public class Modalidade
    {
        public int ModalidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
        public ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();
    }
}