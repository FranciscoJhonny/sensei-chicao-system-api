namespace TorneioSC.Domain.Models
{
    public class Municipio
    {
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public string DescricaoMunicio { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;

        public Estado Estado { get; set; } = new Estado();
        public ICollection<Academia> Academias { get; set; } = new List<Academia>();
        public ICollection<Federacao> Federacoes { get; set; } = new List<Federacao>();
        public ICollection<Torneio> Torneios { get; set; } = new List<Torneio>();
    }
}