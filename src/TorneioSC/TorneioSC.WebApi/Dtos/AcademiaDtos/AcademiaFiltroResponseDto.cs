namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    public class AcademiaFiltroResponseDto
    {
        public IEnumerable<AcademiaDto> Data { get; set; } = new List<AcademiaDto>();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }
    }
}
