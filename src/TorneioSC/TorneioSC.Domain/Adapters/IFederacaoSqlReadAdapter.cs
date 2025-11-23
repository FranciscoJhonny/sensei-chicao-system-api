using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de leitura relacionadas à entidade Federação,
    /// incluindo consultas gerais, filtradas, estatísticas e paginação.
    /// </summary>
    public interface IFederacaoSqlReadAdapter
    {
        /// <summary>
        /// Obtém o total de federações cadastradas, com opção de aplicação de filtros.
        /// </summary>
        /// <param name="filtro">Objeto contendo critérios opcionais de filtro.</param>
        /// <returns>
        /// O número total de federações que atendem ao filtro (ou todas se nenhum filtro for informado).
        /// </returns>
        Task<int> ObterTotalFederacoesAsync(FiltroFederacao? filtro = null);

        /// <summary>
        /// Obtém uma federação pelo ID informado.
        /// </summary>
        /// <param name="federacaoId">ID da federação.</param>
        /// <returns>
        /// A federação correspondente ao ID informado, ou null caso não exista.
        /// </returns>
        Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId);

        /// <summary>
        /// Obtém uma federação pelo CNPJ informado.
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado.</param>
        /// <returns>
        /// A federação encontrada ou null caso o CNPJ não esteja cadastrado.
        /// </returns>
        Task<Federacao?> ObterPorCnpjAsync(string cnpj);

        /// <summary>
        /// Obtém uma federação pelo CNPJ, exceto a federação com o ID informado.
        /// Usado durante atualizações para evitar conflitos.
        /// </summary>
        /// <param name="cnpj">CNPJ a ser pesquisado.</param>
        /// <param name="federacaoId">ID da federação a ser ignorada na pesquisa.</param>
        /// <returns>
        /// A federação encontrada ou null caso não exista outra federação com esse CNPJ.
        /// </returns>
        Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId);

        /// <summary>
        /// Obtém estatísticas gerais relacionadas às federações cadastradas.
        /// </summary>
        /// <returns>
        /// Um objeto contendo diversas métricas e estatísticas agregadas.
        /// </returns>
        Task<EstatisticasFederacoes> ObterEstatisticasFederacoesAsync();

        /// <summary>
        /// Obtém a lista completa de federações cadastradas.
        /// </summary>
        /// <returns>
        /// Uma coleção enumerável contendo todas as federações.
        /// </returns>
        Task<IEnumerable<Federacao>> ObterFederacaoAsync();

        /// <summary>
        /// Obtém uma lista resumida de federações.
        /// </summary>
        /// <returns>
        /// Uma coleção de objetos contendo informações resumidas das federações.
        /// </returns>
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync();

        /// <summary>
        /// Obtém uma lista resumida de federações aplicando filtros.
        /// </summary>
        /// <param name="filtro">Objeto contendo critérios de filtro.</param>
        /// <returns>
        /// Uma coleção de resumos de federações que atendem ao filtro informado.
        /// </returns>
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync(FiltroFederacao filtro);

        /// <summary>
        /// Obtém resumos de federações de forma paginada.
        /// </summary>
        /// <param name="pagina">Número da página (padrão: 1).</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página (padrão: 10).</param>
        /// <returns>
        /// Uma tupla contendo:
        /// - <c>Resumos</c>: coleção de resumos da página atual;
        /// - <c>Total</c>: total de registros existentes.
        /// </returns>
        Task<(IEnumerable<FederacaoResumo> Resumos, int Total)> ObterResumoFederacoesPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);

        /// <summary>
        /// Obtém federações aplicando filtros detalhados.
        /// </summary>
        /// <param name="filtro">Objeto contendo os critérios de filtro.</param>
        /// <returns>
        /// Uma tupla contendo:
        /// - <c>Federacoes</c>: coleção de federações resultante do filtro;
        /// - <c>Total</c>: número total de itens que atendem ao filtro.
        /// </returns>
        Task<(IEnumerable<Federacao> Federacoes, int Total)> ObterFederacoesPorFiltroAsync(FiltroFederacao filtro);
    }

}