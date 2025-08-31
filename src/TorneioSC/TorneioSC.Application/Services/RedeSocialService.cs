using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas à entidade RedeSocial.
    /// Coordena chamadas ao adaptador de leitura para obter redes sociais do sistema.
    /// </summary>
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialSqlReadAdapter _redeSocialSqlReadAdapter;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="RedeSocialService"/>.
        /// </summary>
        /// <param name="redeSocialSqlReadAdapter">Adaptador de leitura para operações no banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="redeSocialSqlReadAdapter"/> for nulo.</exception>
        public RedeSocialService(IRedeSocialSqlReadAdapter redeSocialSqlReadAdapter)
        {
            _redeSocialSqlReadAdapter = redeSocialSqlReadAdapter
                ?? throw new ArgumentNullException(nameof(redeSocialSqlReadAdapter));
        }

        /// <summary>
        /// Obtém todas as redes sociais ativas do sistema.
        /// </summary>
        /// <returns>Lista de todas as redes sociais cadastradas.</returns>
        public async Task<IEnumerable<RedeSocial>> ObterRedeSocialAsync()
        {
            return await _redeSocialSqlReadAdapter.ObterRedeSocialAsync();
        }

        /// <summary>
        /// Obtém uma rede social específica pelo seu ID.
        /// </summary>
        /// <param name="redeSocialId">ID da rede social a ser recuperada.</param>
        /// <returns>A rede social encontrada, ou <c>null</c> se não existir.</returns>
        public async Task<RedeSocial?> ObterRedeSocialPorIdAsync(int redeSocialId)
        {
            return await _redeSocialSqlReadAdapter.ObterRedeSocialPorIdAsync(redeSocialId);
        }
    }
}