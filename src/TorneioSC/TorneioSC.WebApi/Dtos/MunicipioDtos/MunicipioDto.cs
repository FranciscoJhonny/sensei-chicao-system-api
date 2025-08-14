using TorneioSC.WebApi.Dtos.EstadoDtos;

namespace TorneioSC.WebApi.Dtos.MunicipioDtos
{
    public class MunicipioDto
    {
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public string DescricaoMunicio { get; set; } = string.Empty;
       // public EstadoDto Estado { get; set; } = new EstadoDto();
    }
}