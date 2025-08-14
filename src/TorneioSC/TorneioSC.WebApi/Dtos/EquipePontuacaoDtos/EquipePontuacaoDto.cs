using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EquipePontuacaoDtos
{
    public class EquipePontuacaoDto
    {
        public int EquipePontuacaoId { get; set; }
        public int TorneioId { get; set; }
        public int AcademiaId { get; set; }
        public decimal PontuacaoTotal { get; set; } = 0;
        public int? PosicaoFinal { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public TorneioDto Torneio { get; set; } = new TorneioDto();
        public AcademiaDto Academia { get; set; } = new AcademiaDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
