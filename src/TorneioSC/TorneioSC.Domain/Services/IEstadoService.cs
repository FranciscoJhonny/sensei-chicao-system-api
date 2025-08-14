using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Services
{
    public interface IEstadoService
    {
        Task<IEnumerable<Estado>> ObterEstadoAsync();
        Task<Estado?> ObterEstadoPorIdAsync(int estadoId);
    }
}
