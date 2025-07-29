using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Services
{
    public interface IFederacaoService
    {
        Task<IEnumerable<Federacao>> ObterFederacaoAsync();
        Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId);
        Task<Federacao?> ObterPorCnpjAsync(string cnpj);
        Task<int> PostFederacaoAsync(Federacao federacao, int usuarioLogadoId);
        Task<int> UpdateFederacaoAsync(Federacao federacao, int usuarioLogadoId);
        Task<bool> DeleteFederacaoPorIdAsync(int federacaoId);        
        //Task<int> VincularEnderecoAsync(int federacaoId, int enderecoId, int usuarioId);
        //Task<int> VincularTelefoneAsync(int federacaoId, int telefoneId, int usuarioId);
    }
}
