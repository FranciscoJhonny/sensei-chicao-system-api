using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de leitura relacionadas à entidade Perfil
    /// no banco de dados.
    /// </summary>
    public interface IPerfilSqlReadAdapter
    {
        /// <summary>
        /// Obtém um perfil pelo ID informado.
        /// </summary>
        /// <param name="PerfioId">ID do perfil que deve ser consultado.</param>
        /// <returns>
        /// Retorna o perfil encontrado ou null caso não exista um perfil
        /// com o ID informado.
        /// </returns>
        Task<Perfil?> ObterPerfilPorIdAsync(int PerfioId);
    }
}
