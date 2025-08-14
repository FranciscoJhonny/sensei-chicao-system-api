using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IFederacaoSqlReadAdapter
    {
        Task<IEnumerable<Federacao>> ObterFederacaoAsync();
        Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId);
        Task<Federacao?> ObterPorCnpjAsync(string cnpj);
        Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId);
        Task<int> PostFederacaoAsync(Federacao federacao);
        Task<int> PutFederacaoAsync(Federacao federacao);
        Task<bool> DeleteFederacaoPorIdAsync(int federacaoId);
        //Task<int> VincularEnderecoAsync(int federacaoId, int enderecoId, int usuarioId);
        //Task<int> VincularTelefoneAsync(int federacaoId, int telefoneId, int usuarioId);
    }
}