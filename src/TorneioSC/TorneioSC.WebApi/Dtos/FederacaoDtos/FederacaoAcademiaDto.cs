using TorneioSC.WebApi.Dtos.AtletaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;
namespace TorneioSC.WebApi.Dtos.FederacaoAcademiaDtos
{
    public class FederacaoAcademiaDto
    {
        public int AcademiaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? FederacaoId { get; set; }

        public int MunicipioId { get; set; }        
        public FederacaoDto? Federacao { get; set; }
        public FederacaoMunicipioDto Municipio { get; set; } = new FederacaoMunicipioDto();
        public ICollection<EnderecoDto> Enderecos { get; set; } = new List<EnderecoDto>();
        public ICollection<TelefoneDto> Telefones { get; set; } = new List<TelefoneDto>();
        public ICollection<AtletaDto> Atletas { get; set; } = new List<AtletaDto>();
        
    }
}
