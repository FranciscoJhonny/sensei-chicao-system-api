using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IFederacaoSqlWriteAdapter
    {       
        Task<int> PostFederacaoAsync(Federacao federacao);
        Task<int> PutFederacaoAsync(Federacao federacao);
        Task<bool> InativarFederacaoPorIdAsync(int federacaoId, int usuarioOperacaoId);
    }
}