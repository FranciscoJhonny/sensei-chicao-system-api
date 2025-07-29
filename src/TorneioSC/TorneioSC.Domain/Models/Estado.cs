namespace TorneioSC.Domain.Models
{
    public class Estado
    {
        public int EstadoId { get; set; }
        public string DescricaoEstado { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public DateTime? DataOperacao { get; set; }

        public ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
    }
}