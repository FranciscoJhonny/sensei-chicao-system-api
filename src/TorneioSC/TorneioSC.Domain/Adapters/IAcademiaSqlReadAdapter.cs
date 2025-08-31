using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Adapters
{
    public interface IAcademiaSqlReadAdapter
    {
        /// <summary>
        /// Obtém uma academia pelo CNPJ
        /// </summary>
        Task<Academia?> ObterPorCnpjAsync(string Cnpj);
        /// <summary>
        /// Obtém todas as academias ativas
        /// </summary>
        Task<IEnumerable<Academia>> ObterAcademiasAsync();
        /// <summary>
        /// Obtém uma academia específica por ID
        /// </summary>
        Task<Academia?> ObterAcademiaPorIdAsync(int academiaId);  
        /// <summary>
        /// Obtém uma academia pelo CNPJ, excluindo um ID específico (para validação em updates)
        /// </summary>
        Task<Academia?> ObterPorCnpjUpdateAsync(string Cnpj, int academiaId);
        /// <summary>
        /// Obtém academias por filtros (opcional)
        /// </summary>
        Task<(IEnumerable<Academia> Academias, int Total)> ObterAcademiasPorFiltroAsync(FiltroAcademia filtro);  
        /// <summary>
        /// Obtém o método para retornar o total de academias ativas no sistema
        /// </summary>
        Task<int> ObterTotalAcademiasAtivasAsync();
        /// <summary>
        /// Obtém o resumo por paginação
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        Task<(IEnumerable<AcademiaResumo> Resumos, int Total)> ObterResumoAcademiasPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);
        /// <summary>
        /// Obtem academia por filtro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync(FiltroAcademia filtro);
        /// <summary>
        /// Obtem total de academia
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Task<int> ObterTotalAcademiasAsync(FiltroAcademia? filtro = null);
        /// <summary>
        /// Obtem estatisticas de academias
        /// </summary>
        /// <returns></returns>
        Task<EstatisticasAcademias> ObterEstatisticasAcademiasAsync();
        Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync();
    }
}
