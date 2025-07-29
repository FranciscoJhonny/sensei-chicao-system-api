using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioLogadoVM?> LoginUsuario(string login, string senha);
        Task<IEnumerable<Usuario>> ObterUsuarioAsync();
        Task<Usuario> PostUsuario(Usuario usuario, int usuarioLogadoId);
        Task<Usuario?> ObterUsuarioPorIdAsync(int usuarioId);
        Task<int> VerificaUsuarioAsync(string email);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task SalvarTokenRedefinicaoAsync(int UsuarioId, string token, DateTime validade);
        Task<Usuario> UpdateUsuario(Usuario usuario, int usuarioLogadoId);
        Task<bool> DeleteUsuarioPorIdAsync(int usuarioId);
        Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token);
        Task AtualizarSenhaAsync(int usuarioId, string senhaHash);
        Task LimparTokenRecuperacaoAsync(int usuarioId);
    }
}
