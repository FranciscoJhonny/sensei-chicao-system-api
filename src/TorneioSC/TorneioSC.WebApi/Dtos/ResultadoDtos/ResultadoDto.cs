using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ResultadoDtos
{
    public class ResultadoDto
    {
        public int ResultadoId { get; set; }
        public int InscricaoId { get; set; }
        public int Posicao { get; set; }
        public decimal? Pontuacao { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public InscricaoDto Inscricao { get; set; } = new InscricaoDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
