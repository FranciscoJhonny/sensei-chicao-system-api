namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    public class FederacaoFiltroResponseDto
    {
        public IEnumerable<FederacaoDto> Data { get; set; } = new List<FederacaoDto>();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }
    }
}
