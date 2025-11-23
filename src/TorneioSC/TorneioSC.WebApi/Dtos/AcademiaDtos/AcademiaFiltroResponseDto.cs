namespace TorneioSC.WebApi.Dtos.AcademiaDtos
{
    /// <summary>
    /// DTO de resposta para operações de filtro e paginação de academias
    /// </summary>
    public class AcademiaFiltroResponseDto
    {
        /// <summary>
        /// Lista de academias retornadas pela consulta
        /// </summary>
        public IEnumerable<AcademiaDto> Data { get; set; } = new List<AcademiaDto>();

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