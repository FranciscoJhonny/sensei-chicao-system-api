namespace TorneioSC.Domain.Models
{
    /// <summary>
    /// Representa um evento competitivo de karatê no sistema.
    /// Um torneio possui nome, datas, localização, tipo (ex: Estadual, Regional) e está vinculado a um município.
    /// </summary>
    /// <remarks>
    /// A entidade Torneio é central no sistema, pois agrega inscrições, chaves, resultados e estatísticas.
    /// É utilizada para organizar competições por categoria e modalidade (Kata ou Kumite).
    /// </remarks>
    public class Torneio
    {
        public int TorneioId { get; set; }
        public string NomeTorneio { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime TorneioDataInicio { get; set; }
        public DateTime TorneioDataFim { get; set; }
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
        public string DescricaoMunicipio { get; set; } = string.Empty;
        public string DescricaoEstado { get; set; } = string.Empty;
        public ICollection<AcademiaTorneio> Academias { get; set; } = new List<AcademiaTorneio>();
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
        public ICollection<Chaveamento> Chaveamentos { get; set; } = new List<Chaveamento>();
        public ICollection<EquipePontuacao> EquipePontuacoes { get; set; } = new List<EquipePontuacao>();
        public ICollection<EstatisticaPosEvento> EstatisticasPosEvento { get; set; } = new List<EstatisticaPosEvento>();
        public ICollection<EstatisticaPreEvento> EstatisticasPreEvento { get; set; } = new List<EstatisticaPreEvento>();
        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();


    }
}