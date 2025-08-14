using System.Security.Cryptography.Xml;
using TorneioSC.WebApi.Dtos.AcademiaDtos;

namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoMunicipioDto
    {
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public string DescricaoMunicio { get; set; } = string.Empty;
        public FederacaoEstadoDto Estado  { get; set; } = new FederacaoEstadoDto();
        public ICollection<AcademiaDto> Academias { get; set; } = new List<AcademiaDto>();
    }
}