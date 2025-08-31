using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de leitura e escrita de Academia no sistema.
    /// </summary>
    public interface IAcademiaService
    {
        #region 🔽 Métodos de Escrita
        /// <summary>
        /// Cria uma nova academia no sistema
        /// </summary>
        /// <param name="academia">Objeto Academia contendo os dados para criação</param>
        /// <returns>ID da academia criada</returns>
        /// <exception cref="ArgumentException">Lançada quando os dados da academia são inválidos</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de criação</exception>
        Task<int> PostAcademiaAsync(Academia academia);

        /// <summary>
        /// Atualiza os dados de uma academia existente
        /// </summary>
        /// <param name="academia">Objeto Academia contendo os dados atualizados</param>
        /// <returns>Número de registros afetados (1 se atualizado com sucesso, 0 se não encontrado)</returns>
        /// <exception cref="ArgumentException">Lançada quando os dados da academia são inválidos ou ID não informado</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de atualização</exception>
        Task<int> PutAcademiaAsync(Academia academia);

        /// <summary>
        /// Exclui uma academia do sistema (exclusão lógica ou física)
        /// </summary>
        /// <param name="academiaId">ID da academia a ser excluída</param>
        /// <returns>True se a academia foi excluída com sucesso, False se não foi encontrada</returns>
        /// <exception cref="ArgumentException">Lançada quando o ID da academia é inválido</exception>
        /// <exception cref="OperacaoAcademiaException">Lançada quando ocorre erro na operação de exclusão</exception>
        Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId);

        #endregion

        #region 🔽 Métodos de Leitura

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
        Task<Academia?> ObterPorCnpjUpdateAsync(string cnpj, int academiaId);

        /// <summary>
        /// Obtém academias por filtros (opcional)
        /// </summary>
        Task<(IEnumerable<Academia> Academias, int Total)> ObterAcademiasPorFiltroAsync(FiltroAcademia filtro);

        /// <summary>
        /// Obtém academias com informações básicas para listagem
        /// </summary>
        Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync();

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

        #endregion
    }
}
