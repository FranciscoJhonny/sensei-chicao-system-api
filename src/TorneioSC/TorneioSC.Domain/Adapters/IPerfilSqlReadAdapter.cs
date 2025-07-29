using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IPerfilSqlReadAdapter
    {
        Task<Perfil?> ObterPerfilPorIdAsync(int PerfioId);       

    }
}
