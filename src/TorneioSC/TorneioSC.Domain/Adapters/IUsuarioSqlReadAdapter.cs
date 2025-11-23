using TorneioSC.Domain.Models;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Define operações de leitura e escrita relacionadas à entidade Usuário
    /// no banco de dados, incluindo autenticação, consultas, cadastro,
    /// verificação de existência e fluxo de recuperação de senha.
    /// </summary>
    public interface IUsuarioSqlReadAdapter
    {
        /// <summary>
        /// Obtém um usuário com base no login e senha informados.
        /// </summary>
        /// <param name="login">Login do usuário.</param>
        /// <param name="senha">Senha em texto puro a ser validada.</param>
        /// <returns>Retorna o usuário encontrado ou null se não existir.</returns>
        Task<Usuario?> ObterPorUsuarioSenhaAsync(string login, string senha);

        /// <summary>
        /// Obtém uma lista com todos os usuários cadastrados.
        /// </summary>
        /// <returns>Uma coleção enumerável de usuários.</returns>
        Task<IEnumerable<Usuario>> ObterUsuarioAsync();

        /// <summary>
        /// Insere um novo usuário no banco de dados.
        /// </summary>
        /// <param name="usuario">Objeto contendo os dados do novo usuário.</param>
        /// <returns>Retorna o ID gerado para o usuário inserido.</returns>
        Task<int> PostUsuarioAsync(Usuario usuario);

        /// <summary>
        /// Obtém um usuário pelo endereço de e-mail.
        /// </summary>
        /// <param name="email">E-mail a ser pesquisado.</param>
        /// <returns>Retorna o usuário encontrado ou null.</returns>
        Task<Usuario?> ObterPorEmailAsync(string email);

        /// <summary>
        /// Obtém um usuário pelo e-mail, exceto o usuário cujo ID foi informado.
        /// Usado para validação em atualizações.
        /// </summary>
        /// <param name="email">E-mail a ser pesquisado.</param>
        /// <param name="usuarioId">ID do usuário que deve ser ignorado na pesquisa.</param>
        /// <returns>Retorna o usuário encontrado ou null.</returns>
        Task<Usuario?> ObterPorEmailUpdateAsync(string email, int usuarioId);

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <returns>Retorna o usuário encontrado ou null.</returns>
        Task<Usuario?> ObterUsuarioPorIdAsync(int usuarioId);

        /// <summary>
        /// Atualiza os dados de um usuário.
        /// </summary>
        /// <param name="usuario">Objeto contendo os dados atualizados do usuário.</param>
        /// <returns>Retorna 1 se a atualização foi realizada; caso contrário 0.</returns>
        Task<int> UpdateUsuario(Usuario usuario);

        /// <summary>
        /// Remove um usuário do banco de dados.
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser removido.</param>
        /// <returns>Retorna true se o usuário foi excluído; caso contrário false.</returns>
        Task<bool> DeleteUsuarioPorIdAsync(int usuarioId);

        /// <summary>
        /// Verifica se existe um usuário com o e-mail informado.
        /// </summary>
        /// <param name="email">E-mail a ser verificado.</param>
        /// <returns>Retorna 1 se existir, ou 0 se não existir.</returns>
        Task<int> VerificaUsuarioAsync(string email);

        /// <summary>
        /// Salva um token temporário de redefinição de senha.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <param name="token">Token gerado.</param>
        /// <param name="validade">Data e hora de expiração do token.</param>
        Task SalvarTokenRedefinicaoAsync(int usuarioId, string token, DateTime validade);

        /// <summary>
        /// Obtém um usuário pelo token de recuperação de senha.
        /// </summary>
        /// <param name="token">Token de recuperação.</param>
        /// <returns>Retorna o usuário encontrado ou null.</returns>
        Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token);

        /// <summary>
        /// Atualiza a senha hash de um usuário.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <param name="senhaHash">Nova senha já criptografada (hash).</param>
        Task AtualizarSenhaAsync(int usuarioId, string senhaHash);

        /// <summary>
        /// Limpa o token temporário de recuperação de senha após uso.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        Task LimparTokenRecuperacaoAsync(int usuarioId);
    }
}
