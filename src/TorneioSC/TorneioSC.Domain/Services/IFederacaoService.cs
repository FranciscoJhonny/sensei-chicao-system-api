using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de leitura e escrita de Federações no sistema.
    /// </summary>
    public interface IFederacaoService
    {
        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Cria uma nova federação no sistema.
        /// </summary>
        /// <param name="federacao">Objeto contendo os dados da federação a ser criada.</param>
        /// <returns>O ID da federação criada.</returns>
        /// <exception cref="ValidacaoFederacaoException">Lançada quando os dados da federação são inválidos.</exception>
        /// <exception cref="CnpjEmUsoException">Lançada quando o CNPJ já está em uso por outra federação.</exception>
        Task<int> PostFederacaoAsync(Federacao federacao);

        /// <summary>
        /// Atualiza os dados de uma federação existente.
        /// </summary>
        /// <param name="federacao">Objeto contendo os dados atualizados da federação.</param>
        /// <returns>Número de linhas afetadas (geralmente 1 se sucesso, 0 se não encontrado).</returns>
        /// <exception cref="ValidacaoFederacaoException">Lançada quando os dados são inválidos.</exception>
        Task<int> PutFederacaoAsync(Federacao federacao);

        /// <summary>
        /// Inativa uma federação logicamente (exclusão suave).
        /// </summary>
        /// <param name="federacaoId">ID da federação a ser inativada.</param>
        /// <param name="usuarioOperacaoId">ID do usuário que está realizando a operação.</param>
        /// <returns>True se a federação foi inativada com sucesso; caso contrário, false.</returns>
        Task<bool> InativarFederacaoPorIdAsync(int federacaoId, int usuarioOperacaoId);

        #endregion

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém o total de federações ativas, com base em filtros opcionais.
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis (nome, CNPJ, município, estado, status).</param>
        /// <returns>Número total de federações que atendem aos critérios.</returns>
        Task<int> ObterTotalFederacoesAsync(FiltroFederacao? filtro = null);

        /// <summary>
        /// Obtém uma federação pelo seu ID.
        /// </summary>
        /// <param name="federacaoId">ID da federação.</param>
        /// <returns>A federação encontrada ou null se não existir.</returns>
        Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId);

        /// <summary>
        /// Obtém uma federação pelo CNPJ (somente ativas).
        /// </summary>
        /// <param name="cnpj">CNPJ da federação (com ou sem máscara).</param>
        /// <returns>Federação encontrada ou null se não existir.</returns>
        Task<Federacao?> ObterPorCnpjAsync(string cnpj);

        /// <summary>
        /// Verifica se um CNPJ já está em uso por outra federação (usado em atualizações).
        /// </summary>
        /// <param name="cnpj">CNPJ a ser verificado.</param>
        /// <param name="federacaoId">ID da federação que está sendo editada (excluída da verificação).</param>
        /// <returns>Federação encontrada com o CNPJ ou null se disponível.</returns>
        Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId);

        /// <summary>
        /// Obtém estatísticas gerais das federações (total, ativas, inativas, estados, cidades).
        /// </summary>
        /// <returns>Objeto com as estatísticas detalhadas.</returns>
        Task<EstatisticasFederacoes> ObterEstatisticasFederacoesAsync();

        /// <summary>
        /// Obtém uma lista paginada de federações com todos os dados, aplicando filtros.
        /// </summary>
        /// <param name="filtro">Filtros e paginação (nome, CNPJ, município, estado, ativo, página, tamanho).</param>
        /// <returns>Tupla contendo a lista de federações e o total de registros (para paginação).</returns>
        Task<(IEnumerable<Federacao> Federacoes, int Total)> ObterFederacoesPorFiltroAsync(FiltroFederacao filtro);

        /// <summary>
        /// Obtém todas as federações ativas do sistema.
        /// </summary>
        /// <returns>Lista de todas as federações ativas.</returns>
        Task<IEnumerable<Federacao>> ObterFederacaoAsync();

        /// <summary>
        /// Obtém um resumo básico de todas as federações (ID, Nome, CNPJ, Cidade, Ativo).
        /// </summary>
        /// <returns>Lista de resumos das federações.</returns>
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync();

        /// <summary>
        /// Obtém um resumo de federações filtradas (por nome, CNPJ, município, estado).
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis.</param>
        /// <returns>Lista de resumos das federações que atendem aos critérios.</returns>
        Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync(FiltroFederacao filtro);

        /// <summary>
        /// Obtém um resumo de federações com paginação.
        /// </summary>
        /// <param name="pagina">Página atual (começa em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de registros por página (máx: 100).</param>
        /// <returns>Tupla contendo os resumos e o total de registros.</returns>
        Task<(IEnumerable<FederacaoResumo> Resumos, int Total)> ObterResumoFederacoesPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);

        #endregion
    }
}