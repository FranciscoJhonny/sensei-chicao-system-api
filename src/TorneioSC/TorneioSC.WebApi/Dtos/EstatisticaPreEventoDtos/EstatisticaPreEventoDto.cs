using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EstatisticaPreEventoDtos
{
    public class EstatisticaPreEventoDto
    {
        public int EstatisticaId { get; set; }
        public int TorneioId { get; set; }
        public int TotalAtletas { get; set; }
        public int TotalAcademias { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalModalidades { get; set; }
        public int EstimativaMedalhas { get; set; }
        public DateTime GeradoEm { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public TorneioDto Torneio { get; set; } = new TorneioDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
