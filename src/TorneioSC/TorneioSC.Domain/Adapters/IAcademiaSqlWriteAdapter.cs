using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IAcademiaSqlWriteAdapter
    {
        Task<int> PostAcademiaAsync(Academia academia);
        Task<int> PutAcademiaAsync(Academia academia);
        Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId);
    }
}
