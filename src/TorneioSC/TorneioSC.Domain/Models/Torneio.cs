namespace TorneioSC.Domain.Models
{
    public class Torneio
    {
        public int TorneioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int MunicipioId { get; set; }
        public string? Contratante { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public Municipio Municipio { get; set; } = new Municipio();
        public Usuario? UsuarioInclusao { get; set; }
        public Usuario? UsuarioOperacao { get; set; }
        public ICollection<AcademiaTorneio> Academias { get; set; } = new List<AcademiaTorneio>();
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
        public ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();
        public ICollection<EquipePontuacao> EquipePontuacoes { get; set; } = new List<EquipePontuacao>();
        public ICollection<EstatisticaPosEvento> EstatisticasPosEvento { get; set; } = new List<EstatisticaPosEvento>();
        public ICollection<EstatisticaPreEvento> EstatisticasPreEvento { get; set; } = new List<EstatisticaPreEvento>();
        public ICollection<EventoTorneio> Eventos { get; set; } = new List<EventoTorneio>();
    }
}