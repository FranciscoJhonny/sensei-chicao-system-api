using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoSqlReadAdapter _EstadoSqlAdapter;

        public EstadoService(IEstadoSqlReadAdapter EstadoSqlAdapter)
        {
            _EstadoSqlAdapter = EstadoSqlAdapter;
        }

        public async Task<IEnumerable<Estado>> ObterEstadoAsync()
        {
            return await _EstadoSqlAdapter.ObterEstadoAsync();
        }

        public async Task<Estado?> ObterEstadoPorIdAsync(int estadoId)
        {
            return await _EstadoSqlAdapter.ObterEstadoPorIdAsync(estadoId);
        }
    }
}
