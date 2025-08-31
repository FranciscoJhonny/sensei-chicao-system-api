using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Interface do adaptador de leitura para operações relacionadas à entidade TipoTelefone.
    /// Define os contratos para obtenção de tipos de telefone do sistema.
    /// </summary>
    public interface ITipoTelefoneSqlReadAdapter
    {
        /// <summary>
        /// Obtém todos os tipos de telefone ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os tipos de telefone cadastrados.</returns>
        Task<IEnumerable<TipoTelefone>> ObterTipoTelefoneAsync();

        /// <summary>
        /// Obtém um tipo de telefone específico pelo seu ID.
        /// </summary>
        /// <param name="tipoTelefoneId">ID do tipo de telefone a ser recuperado.</param>
        /// <returns>O tipo de telefone encontrado, ou <c>null</c> se não existir.</returns>
        Task<TipoTelefone?> ObterTipoTelefonePorIdAsync(int tipoTelefoneId);
    }
}