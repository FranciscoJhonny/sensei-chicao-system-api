using TorneioSC.WebApi.Dtos.ChaveamentoDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ModalidadeDtos
{
    public class ModalidadeDto
    {
        public int ModalidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
        public ICollection<InscricaoDto> Inscricoes { get; set; } = new List<InscricaoDto>();
        public ICollection<ChaveamentoDto> Chaveamentos { get; set; } = new List<ChaveamentoDto>();
    }
}
