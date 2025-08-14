using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IMunicipioSqlReadAdapter
    {
        Task<IEnumerable<Municipio>> ObterMunicipioAsync();
        Task<Municipio?> ObterMunicipioPorIdAsync(int municipioId);
        Task<IEnumerable<Municipio>> ObterMunicipioPorEstadoIdAsync(int estadoId);

    }
}
