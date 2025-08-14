using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.EstatisticaPosEventoDtos
{
    public class EstatisticaPosEventoDto
    {
        public int EstatisticaId { get; set; }
        public int TorneioId { get; set; }
        public int MedalhasDistribuidas { get; set; }
        public int CertificadosEmitidos { get; set; }
        public int TotalLutas { get; set; }
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
