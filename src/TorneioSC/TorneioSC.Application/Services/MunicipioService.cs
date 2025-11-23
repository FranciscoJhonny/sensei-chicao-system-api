using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço para gerenciamento de municípios
    /// </summary>
    public class MunicipioService : IMunicipioService
    {
        private readonly IMunicipioSqlReadAdapter _municipioSqlAdapter;

        /// <summary>
        /// Construtor do serviço de municípios
        /// </summary>
        /// <param name="municipioSqlAdapter">Adapter para operações de leitura de municípios</param>
        public MunicipioService(IMunicipioSqlReadAdapter municipioSqlAdapter)
        {
            _municipioSqlAdapter = municipioSqlAdapter;
        }

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém todos os municípios cadastrados
        /// </summary>
        /// <returns>Lista de todos os municípios</returns>
        public async Task<IEnumerable<Municipio>> ObterMunicipioAsync()
        {
            return await _municipioSqlAdapter.ObterMunicipioAsync();
        }

        /// <summary>
        /// Obtém um município específico pelo seu ID
        /// </summary>
        /// <param name="municipioId">ID do município a ser obtido</param>
        /// <returns>Município encontrado ou null se não existir</returns>
        public async Task<Municipio?> ObterMunicipioPorIdAsync(int municipioId)
        {
            return await _municipioSqlAdapter.ObterMunicipioPorIdAsync(municipioId);
        }

        /// <summary>
        /// Obtém todos os municípios pertencentes a um estado específico
        /// </summary>
        /// <param name="estadoId">ID do estado para filtrar os municípios</param>
        /// <returns>Lista de municípios do estado especificado</returns>
        public async Task<IEnumerable<Municipio>> ObterMunicipioPorEstadoIdAsync(int estadoId)
        {
            return await _municipioSqlAdapter.ObterMunicipioPorEstadoIdAsync(estadoId);
        }

        #endregion

        #region 🔽 Métodos de Escrita

        // Como a MunicipioService atual só tem métodos de leitura,
        // esta região ficará vazia até que métodos de escrita sejam adicionados
        // Exemplo futuro:
        // public async Task<int> PostMunicipioAsync(Municipio municipio) { ... }
        // public async Task<int> PutMunicipioAsync(Municipio municipio) { ... }
        // public async Task<bool> InativarMunicipioPorIdAsync(int municipioId) { ... }

        #endregion

        #region 🔽 Validações

        // Como a MunicipioService atual só tem métodos de leitura,
        // esta região ficará vazia até que validações sejam necessárias
        // Exemplo futuro:
        // private List<string> ValidarMunicipio(Municipio municipio) { ... }
        // private bool IsValidCodigoIBGE(string codigoIBGE) { ... }

        #endregion
    }
}