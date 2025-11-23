using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço para gerenciamento de estados
    /// </summary>
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoSqlReadAdapter _estadoSqlAdapter;

        /// <summary>
        /// Construtor do serviço de estados
        /// </summary>
        /// <param name="estadoSqlAdapter">Adapter para operações de leitura de estados</param>
        public EstadoService(IEstadoSqlReadAdapter estadoSqlAdapter)
        {
            _estadoSqlAdapter = estadoSqlAdapter ?? throw new ArgumentNullException(nameof(estadoSqlAdapter));
        }

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém todos os estados ativos do sistema
        /// </summary>
        /// <returns>Lista de todos os estados cadastrados</returns>
        public async Task<IEnumerable<Estado>> ObterEstadoAsync()
        {
            return await _estadoSqlAdapter.ObterEstadoAsync();
        }

        /// <summary>
        /// Obtém um estado específico pelo seu ID, incluindo os municípios associados
        /// </summary>
        /// <param name="estadoId">ID do estado a ser recuperado</param>
        /// <returns>O estado encontrado com seus municípios, ou null se não existir</returns>
        public async Task<Estado?> ObterEstadoPorIdAsync(int estadoId)
        {
            return await _estadoSqlAdapter.ObterEstadoPorIdAsync(estadoId);
        }

        #endregion

        #region 🔽 Métodos de Escrita

        // Como a EstadoService atual só tem métodos de leitura,
        // esta região ficará vazia até que métodos de escrita sejam adicionados
        // Exemplo futuro:
        // public async Task<int> PostEstadoAsync(Estado estado) { ... }
        // public async Task<int> PutEstadoAsync(Estado estado) { ... }
        // public async Task<bool> InativarEstadoPorIdAsync(int estadoId) { ... }

        #endregion

        #region 🔽 Validações

        // Como a EstadoService atual só tem métodos de leitura,
        // esta região ficará vazia até que validações sejam necessárias
        // Exemplo futuro:
        // private List<string> ValidarEstado(Estado estado) { ... }
        // private bool IsValidSigla(string sigla) { ... }

        #endregion
    }
}