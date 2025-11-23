using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de leitura relacionadas à entidade Município
    /// no banco de dados.
    /// </summary>
    public interface IMunicipioSqlReadAdapter
    {
        /// <summary>
        /// Obtém a lista completa de municípios cadastrados.
        /// </summary>
        /// <returns>
        /// Uma coleção enumerável contendo todos os municípios.
        /// </returns>
        Task<IEnumerable<Municipio>> ObterMunicipioAsync();

        /// <summary>
        /// Obtém um município pelo ID informado.
        /// </summary>
        /// <param name="municipioId">ID do município a ser consultado.</param>
        /// <returns>
        /// O município correspondente ao ID informado, ou null caso não exista.
        /// </returns>
        Task<Municipio?> ObterMunicipioPorIdAsync(int municipioId);

        /// <summary>
        /// Obtém todos os municípios pertencentes a um estado específico.
        /// </summary>
        /// <param name="estadoId">ID do estado.</param>
        /// <returns>
        /// Uma coleção de municípios associados ao estado informado.
        /// </returns>
        Task<IEnumerable<Municipio>> ObterMunicipioPorEstadoIdAsync(int estadoId);
    }

}
