using TorneioSC.WebApi.Dtos.MunicipioDtos;

namespace TorneioSC.WebApi.Dtos.EstadoDtos
{
    public class EstadoDto
    {
        public int EstadoId { get; set; }
        public string DescricaoEstado { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public ICollection<MunicipioDto> Municipios { get; set; } = new List<MunicipioDto>();
    }
}