using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.ChaveamentoDtos;
using TorneioSC.WebApi.Dtos.EquipePontuacaoDtos;
using TorneioSC.WebApi.Dtos.EstatisticaPosEventoDtos;
using TorneioSC.WebApi.Dtos.EstatisticaPreEventoDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.TorneioDtos
{
    public class TorneioDto
    {
        public int TorneioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int MunicipioId { get; set; }
        public string? Contratante { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        public MunicipioDto Municipio { get; set; } = new MunicipioDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
        public ICollection<AcademiaDto> Academias { get; set; } = new List<AcademiaDto>();
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();
        public ICollection<ChaveamentoDto> Chaveamentos { get; set; } = new List<ChaveamentoDto>();
        public ICollection<EquipePontuacaoDto> EquipePontuacoes { get; set; } = new List<EquipePontuacaoDto>();
        public ICollection<EstatisticaPosEventoDto> EstatisticasPosEvento { get; set; } = new List<EstatisticaPosEventoDto>();
        public ICollection<EstatisticaPreEventoDto> EstatisticasPreEvento { get; set; } = new List<EstatisticaPreEventoDto>();
        public ICollection<CategoriaDto> Categorias { get; set; } = new List<CategoriaDto>();

    }
}
