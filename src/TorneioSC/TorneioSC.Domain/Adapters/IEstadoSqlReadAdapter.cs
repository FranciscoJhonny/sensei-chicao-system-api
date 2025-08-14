using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IEstadoSqlReadAdapter
    {
        Task<IEnumerable<Estado>> ObterEstadoAsync();
        Task<Estado?> ObterEstadoPorIdAsync(int estadoId);

    }
}
