using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Adapters
{
    public interface IFederacaoSqlReadAdapter
    {

        Task<int> ObterTotalFederacoesAsync(FiltroFederacao? filtro = null);
        Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId);
        Task<Federacao?> ObterPorCnpjAsync(string cnpj);
        Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId);
        Task<EstatisticasFederacoes> ObterEstatisticasFederacoesAsync();
        Task<IEnumerable<Federacao>> ObterFederacaoAsync();
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync();        
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync(FiltroFederacao filtro);
        Task<(IEnumerable<FederacaoResumo> Resumos, int Total)> ObterResumoFederacoesPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);
        Task<(IEnumerable<Federacao> Federacoes, int Total)> ObterFederacoesPorFiltroAsync(FiltroFederacao filtro);
    }
}