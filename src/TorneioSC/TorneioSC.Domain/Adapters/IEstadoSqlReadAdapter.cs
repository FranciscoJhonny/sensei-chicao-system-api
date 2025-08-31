using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Interface para operações de leitura relacionadas à entidade Estado.
    /// Define os contratos para obtenção de estados e seus dados associados.
    /// </summary>
    public interface IEstadoSqlReadAdapter
    {
        /// <summary>
        /// Obtém todos os estados ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os estados cadastrados.</returns>
        Task<IEnumerable<Estado>> ObterEstadoAsync();

        /// <summary>
        /// Obtém um estado específico pelo seu ID, incluindo os municípios associados.
        /// </summary>
        /// <param name="estadoId">ID do estado a ser recuperado.</param>
        /// <returns>O estado encontrado com seus municípios, ou <c>null</c> se não existir.</returns>
        Task<Estado?> ObterEstadoPorIdAsync(int estadoId);
    }
}