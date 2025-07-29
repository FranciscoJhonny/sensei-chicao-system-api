using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    public interface IUsuarioSqlReadAdapter
    {
        Task<Usuario?> ObterPorUsuarioSenhaAsync(string login, string senha);
        Task<IEnumerable<Usuario>> ObterUsuarioAsync();
        Task<int> PostUsuarioAsync(Usuario usuario);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorEmailUpdateAsync(string email, int usuarioId);
        Task<Usuario?> ObterUsuarioPorIdAsync(int usuarioId);
        Task<int> UpdateUsuario(Usuario usuario);
        Task<bool> DeleteUsuarioPorIdAsync(int usuarioId);       
        Task<int> VerificaUsuarioAsync(string email);
        Task SalvarTokenRedefinicaoAsync(int usuarioId, string token, DateTime validade);
        Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token);
        Task AtualizarSenhaAsync(int usuarioId, string senhaHash);
        Task LimparTokenRecuperacaoAsync(int usuarioId);

    }
}
