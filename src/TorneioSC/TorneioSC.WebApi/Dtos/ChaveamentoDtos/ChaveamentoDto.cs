using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.ModalidadeDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.ChaveamentoDtos
{
    public class ChaveamentoDto
    {
        public int ChaveamentoId { get; set; }
        public int TorneioId { get; set; }
        public int CategoriaId { get; set; }
        public int ModalidadeId { get; set; }
        public string? DadosChave { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public TorneioDto Torneio { get; set; } = new TorneioDto();
        public CategoriaDto Categoria { get; set; } = new CategoriaDto();
        public ModalidadeDto Modalidade { get; set; } = new ModalidadeDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
    }
}
