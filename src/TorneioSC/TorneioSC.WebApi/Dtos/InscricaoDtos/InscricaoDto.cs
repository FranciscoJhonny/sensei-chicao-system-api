using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.CategoriaDtos;
using TorneioSC.WebApi.Dtos.CertificadoDtos;
using TorneioSC.WebApi.Dtos.ModalidadeDtos;
using TorneioSC.WebApi.Dtos.ResultadoDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi.Dtos.InscricaoDtos
{
    public class InscricaoDto
    {
        public int InscricaoId { get; set; }
        public int AtletaId { get; set; }
        public int TorneioId { get; set; }
        public int CategoriaId { get; set; }
        public int ModalidadeId { get; set; }
        public bool Ativo { get; set; } = true;
        public int? UsuarioInclusaoId { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public string? NaturezaOperacao { get; set; }
        public int? UsuarioOperacaoId { get; set; }
        public DateTime? DataOperacao { get; set; }

        public AtletaDto Atleta { get; set; } = new AtletaDto();
        public TorneioDto Torneio { get; set; } = new TorneioDto();
        public CategoriaDto Categoria { get; set; } = new CategoriaDto();
        public ModalidadeDto Modalidade { get; set; } = new ModalidadeDto();
        public UsuarioDto? UsuarioInclusao { get; set; }
        public UsuarioDto? UsuarioOperacao { get; set; }
        public ICollection<CertificadoDto> Certificados { get; set; } = new List<CertificadoDto>();
        public ICollection<ResultadoDto> Resultados { get; set; } = new List<ResultadoDto>();
    }
}
