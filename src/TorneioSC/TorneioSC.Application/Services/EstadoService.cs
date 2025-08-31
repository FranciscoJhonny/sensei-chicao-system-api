using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas à entidade Estado.
    /// Coordena chamadas ao adaptador de leitura para obter estados e seus municípios associados.
    /// </summary>
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoSqlReadAdapter _EstadoSqlAdapter;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="EstadoService"/>.
        /// </summary>
        /// <param name="estadoSqlAdapter">Adaptador de leitura para operações no banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="estadoSqlAdapter"/> for nulo.</exception>
        public EstadoService(IEstadoSqlReadAdapter estadoSqlAdapter)
        {
            _EstadoSqlAdapter = estadoSqlAdapter ?? throw new ArgumentNullException(nameof(estadoSqlAdapter));
        }

        /// <summary>
        /// Obtém todos os estados ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os estados cadastrados.</returns>
        public async Task<IEnumerable<Estado>> ObterEstadoAsync()
        {
            return await _EstadoSqlAdapter.ObterEstadoAsync();
        }

        /// <summary>
        /// Obtém um estado específico pelo seu ID, incluindo os municípios associados.
        /// </summary>
        /// <param name="estadoId">ID do estado a ser recuperado.</param>
        /// <returns>O estado encontrado com seus municípios, ou <c>null</c> se não existir.</returns>
        public async Task<Estado?> ObterEstadoPorIdAsync(int estadoId)
        {
            return await _EstadoSqlAdapter.ObterEstadoPorIdAsync(estadoId);
        }
    }
}