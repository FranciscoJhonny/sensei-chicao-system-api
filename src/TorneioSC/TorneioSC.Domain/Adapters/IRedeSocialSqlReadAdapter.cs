using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Interface do adaptador de leitura para operações relacionadas à entidade RedeSocial.
    /// Define os contratos para obtenção de redes sociais do sistema.
    /// </summary>
    public interface IRedeSocialSqlReadAdapter
    {
        /// <summary>
        /// Obtém todas as redes sociais ativas do sistema.
        /// </summary>
        /// <returns>Lista de todas as redes sociais cadastradas.</returns>
        Task<IEnumerable<RedeSocial>> ObterRedeSocialAsync();

        /// <summary>
        /// Obtém uma rede social específica pelo seu ID.
        /// </summary>
        /// <param name="redeSocialId">ID da rede social a ser recuperada.</param>
        /// <returns>A rede social encontrada, ou <c>null</c> se não existir.</returns>
        Task<RedeSocial?> ObterRedeSocialPorIdAsync(int redeSocialId);
    }
}