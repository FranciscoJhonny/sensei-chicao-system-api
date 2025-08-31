using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas à entidade TipoTelefone.
    /// Coordena chamadas ao adaptador de leitura para obter tipos de telefone do sistema.
    /// </summary>
    public class TipoTelefoneService : ITipoTelefoneService
    {
        private readonly ITipoTelefoneSqlReadAdapter _tipoTelefoneSqlReadAdapter;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TipoTelefoneService"/>.
        /// </summary>
        /// <param name="tipoTelefoneSqlReadAdapter">Adaptador de leitura para operações no banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="tipoTelefoneSqlReadAdapter"/> for nulo.</exception>
        public TipoTelefoneService(ITipoTelefoneSqlReadAdapter tipoTelefoneSqlReadAdapter)
        {
            _tipoTelefoneSqlReadAdapter = tipoTelefoneSqlReadAdapter
                ?? throw new ArgumentNullException(nameof(tipoTelefoneSqlReadAdapter));
        }

        /// <summary>
        /// Obtém todos os tipos de telefone ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os tipos de telefone cadastrados.</returns>
        public async Task<IEnumerable<TipoTelefone>> ObterTipoTelefoneAsync()
        {
            return await _tipoTelefoneSqlReadAdapter.ObterTipoTelefoneAsync();
        }

        /// <summary>
        /// Obtém um tipo de telefone específico pelo seu ID.
        /// </summary>
        /// <param name="tipoTelefoneId">ID do tipo de telefone a ser recuperado.</param>
        /// <returns>O tipo de telefone encontrado, ou <c>null</c> se não existir.</returns>
        public async Task<TipoTelefone?> ObterTipoTelefonePorIdAsync(int tipoTelefoneId)
        {
            return await _tipoTelefoneSqlReadAdapter.ObterTipoTelefonePorIdAsync(tipoTelefoneId);
        }
    }
}