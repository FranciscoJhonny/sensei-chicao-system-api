namespace TorneioSC.WebApi.Dtos.FederacaoDtos
{
    /// <summary>
    /// DTO de resposta para operações de filtro e paginação de federações
    /// </summary>
    public class FederacaoFiltroResponseDto
    {
        /// <summary>
        /// Lista de federações retornadas pela consulta
        /// </summary>
        public IEnumerable<FederacaoDto> Data { get; set; } = new List<FederacaoDto>();

        /// <summary>
        /// Número total de registros encontrados (sem paginação)
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Página atual da consulta
        /// </summary>
        public int Pagina { get; set; }

        /// <summary>
        /// Número de registros por página
        /// </summary>
        public int TamanhoPagina { get; set; }

        /// <summary>
        /// Número total de páginas disponíveis
        /// </summary>
        public int TotalPaginas { get; set; }
    }
}