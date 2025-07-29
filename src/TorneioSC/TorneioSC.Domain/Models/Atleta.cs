namespace TorneioSC.Domain.Models
{
    public class Atleta
    {
        public int AtletaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public decimal Peso { get; set; }
        public int AcademiaId { get; set; }
        public string? CPF { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Academia Academia { get; set; } = new Academia();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
    }
}