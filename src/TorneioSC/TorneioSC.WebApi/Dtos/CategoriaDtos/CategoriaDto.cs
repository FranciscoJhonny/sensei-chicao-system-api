using TorneioSC.WebApi.Dtos.ChaveamentoDtos;
using TorneioSC.WebApi.Dtos.InscricaoDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.CategoriaDtos
{
    public class CategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int IdadeMin { get; set; }
        public int IdadeMax { get; set; }
        public char Sexo { get; set; }
        public decimal? PesoMin { get; set; }
        public decimal? PesoMax { get; set; }
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
