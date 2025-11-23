using Microsoft.Extensions.Logging;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionPerfil;
using TorneioSC.Exception.ExceptionBase.ExceptionUsuario;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço para gerenciamento de usuários
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioSqlReadAdapter _usuarioSqlAdapter;
        private readonly IPerfilSqlReadAdapter _perfilSqlAdapter;
        private readonly ILogger<UsuarioService> _logger;

        /// <summary>
        /// Construtor do serviço de usuários
        /// </summary>
        /// <param name="usuarioSqlAdapter">Adapter para operações de leitura de usuários</param>
        /// <param name="perfilSqlAdapter">Adapter para operações de leitura de perfis</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public UsuarioService(IUsuarioSqlReadAdapter usuarioSqlAdapter, IPerfilSqlReadAdapter perfilSqlAdapter, ILogger<UsuarioService> logger)
        {
            _usuarioSqlAdapter = usuarioSqlAdapter ?? throw new ArgumentNullException(nameof(usuarioSqlAdapter));
            _perfilSqlAdapter = perfilSqlAdapter ?? throw new ArgumentNullException(nameof(perfilSqlAdapter));
            _logger = logger;
        }

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém todos os usuários cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        public async Task<IEnumerable<Usuario>> ObterUsuarioAsync()
        {
            return await _usuarioSqlAdapter.ObterUsuarioAsync();
        }

        /// <summary>
        /// Obtém um usuário pelo ID
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser obtido</param>
        /// <returns>Usuário encontrado ou null se não existir</returns>
        /// <exception cref="ArgumentException">Lançada quando o ID do usuário é inválido</exception>
        public async Task<Usuario?> ObterUsuarioPorIdAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));
            }

            return await _usuarioSqlAdapter.ObterUsuarioPorIdAsync(usuarioId);
        }

        /// <summary>
        /// Verifica se um usuário existe pelo email
        /// </summary>
        /// <param name="email">Email a ser verificado</param>
        /// <returns>1 se o usuário existe, 0 caso contrário</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o email é nulo ou vazio</exception>
        /// <exception cref="EmailInvalidoException">Lançada quando o email está em formato inválido</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task<int> VerificaUsuarioAsync(string email)
        {
            try
            {
                // Validação básica do email
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email não pode ser vazio");
                }

                if (!IsValidEmail(email))
                {
                    throw new EmailInvalidoException(email);
                }

                // Verifica se o email já existe
                return await _usuarioSqlAdapter.VerificaUsuarioAsync(email);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar usuário com email: {Email}", email);
                throw new OperacaoUsuarioException("verificação de usuário", ex);
            }
        }

        /// <summary>
        /// Obtém um usuário pelo email
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Usuário encontrado ou null se não existir</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o email é nulo ou vazio</exception>
        /// <exception cref="EmailInvalidoException">Lançada quando o email está em formato inválido</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            try
            {
                // Validação básica do email
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email não pode ser vazio");
                }

                if (!IsValidEmail(email))
                {
                    throw new EmailInvalidoException(email);
                }

                // Verifica se o email já existe
                return await _usuarioSqlAdapter.ObterPorEmailAsync(email);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar usuário com email: {Email}", email);
                throw new OperacaoUsuarioException("verificação de usuário", ex);
            }
        }

        /// <summary>
        /// Obtém um usuário pelo token de recuperação de senha
        /// </summary>
        /// <param name="token">Token de recuperação</param>
        /// <returns>Usuário encontrado ou null se não existir</returns>
        /// <exception cref="ArgumentException">Lançada quando o token é nulo ou vazio</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Token não pode ser vazia", nameof(token));

                var usuario = await _usuarioSqlAdapter.ObterPorTokenRecuperacaoAsync(token);

                return usuario;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário com o token: {token}");
                throw new OperacaoUsuarioException("busca por ID", ex);
            }
        }

        #endregion

        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Realiza o login de um usuário
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Dados do usuário logado ou null se as credenciais forem inválidas</returns>
        /// <exception cref="ValidacaoUsuarioException">Lançada quando há erros de validação nos parâmetros</exception>
        /// <exception cref="CredenciaisInvalidasException">Lançada quando as credenciais são inválidas</exception>
        /// <exception cref="UsuarioInativoException">Lançada quando o usuário está inativo</exception>
        /// <exception cref="PerfilNaoEncontradoException">Lançada quando o perfil do usuário não é encontrado</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task<UsuarioLogadoVM?> LoginUsuario(string login, string senha)
        {
            try
            {
                // Validações iniciais
                var errosValidacao = new List<string>();

                if (string.IsNullOrWhiteSpace(login))
                    errosValidacao.Add("Login é obrigatório");

                if (string.IsNullOrWhiteSpace(senha))
                    errosValidacao.Add("Senha é obrigatória");

                if (errosValidacao.Any())
                    throw new ValidacaoUsuarioException(errosValidacao);

                string senhaMD5 = Recursos.ObterHashMD5(senha).ToUpper();
                Usuario? usuario = await _usuarioSqlAdapter.ObterPorUsuarioSenhaAsync(login, senhaMD5);

                if (usuario == null)
                    throw new CredenciaisInvalidasException();

                if (!usuario.Ativo)
                    throw new UsuarioInativoException(usuario.UsuarioId);

                Perfil? perfil = await _perfilSqlAdapter.ObterPerfilPorIdAsync(usuario.PerfilId);

                if (perfil == null)
                    throw new PerfilNaoEncontradoException(usuario.PerfilId);

                return new UsuarioLogadoVM
                {
                    UsuarioId = usuario.UsuarioId,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    PerfilId = usuario.PerfilId,
                    DescricaoPerfil = perfil.Descricao
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar realizar login");
                throw new OperacaoUsuarioException("autenticação", ex);
            }
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="usuario">Dados do usuário a ser criado</param>
        /// <param name="usuarioLogadoId">ID do usuário logado que está realizando a operação</param>
        /// <returns>Usuário criado com ID atribuído</returns>
        /// <exception cref="ValidacaoUsuarioException">Lançada quando há erros de validação nos dados do usuário</exception>
        /// <exception cref="UnauthorizedAccessException">Lançada quando o usuário não tem permissão para criar usuários</exception>
        /// <exception cref="EmailEmUsoException">Lançada quando o email já está em uso</exception>
        /// <exception cref="PerfilNaoEncontradoException">Lançada quando o perfil não existe</exception>
        public async Task<Usuario> PostUsuario(Usuario usuario, int usuarioLogadoId)
        {
            // 1. Validar campos obrigatórios
            var errosValidacao = ValidarUsuario(usuario);
            if (errosValidacao.Any())
            {
                throw new ValidacaoUsuarioException(errosValidacao);
            }

            // 2. Verificar permissões
            await ValidarPermissoes(usuarioLogadoId);

            // 3. Verificar regras de negócio
            await ValidarRegrasNegocio(usuario);

            // 4. Preparar e criar usuário
            var novoUsuario = PrepararCriarUsuario(usuario, usuarioLogadoId);
            novoUsuario.UsuarioId = await _usuarioSqlAdapter.PostUsuarioAsync(novoUsuario);

            return novoUsuario;
        }

        /// <summary>
        /// Atualiza os dados de um usuário
        /// </summary>
        /// <param name="usuario">Dados atualizados do usuário</param>
        /// <param name="usuarioLogadoId">ID do usuário logado que está realizando a operação</param>
        /// <returns>Usuário atualizado</returns>
        /// <exception cref="ValidacaoUsuarioException">Lançada quando há erros de validação nos dados do usuário</exception>
        /// <exception cref="UnauthorizedAccessException">Lançada quando o usuário não tem permissão para atualizar usuários</exception>
        /// <exception cref="EmailEmUsoException">Lançada quando o email já está em uso</exception>
        /// <exception cref="PerfilNaoEncontradoException">Lançada quando o perfil não existe</exception>
        public async Task<Usuario> UpdateUsuario(Usuario usuario, int usuarioLogadoId)
        {
            // 1. Validar campos obrigatórios
            var errosValidacao = ValidarUsuario(usuario);
            if (errosValidacao.Any())
            {
                throw new ValidacaoUsuarioException(errosValidacao);
            }

            // 2. Verificar permissões
            await ValidarPermissoes(usuarioLogadoId);

            // 3. Verificar regras de negócio
            await ValidarRegrasNegocioUpdate(usuario);
            // 4. Preparar e alterar usuário
            var novoUsuario = PrepararUpdateUsuario(usuario, usuarioLogadoId);

            novoUsuario.UsuarioId = await _usuarioSqlAdapter.UpdateUsuario(novoUsuario);

            return novoUsuario;
        }

        /// <summary>
        /// Exclui um usuário pelo ID
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser excluído</param>
        /// <returns>True se a exclusão foi bem-sucedida, false caso contrário</returns>
        /// <exception cref="ArgumentException">Lançada quando o ID do usuário é inválido</exception>
        public async Task<bool> DeleteUsuarioPorIdAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));
            }
            return await _usuarioSqlAdapter.DeleteUsuarioPorIdAsync(usuarioId);
        }

        /// <summary>
        /// Salva o token de redefinição de senha para um usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="token">Token de redefinição</param>
        /// <param name="validade">Data de validade do token</param>
        /// <exception cref="ArgumentException">Lançada quando o ID do usuário é inválido</exception>
        /// <exception cref="SalvarTokenRedefinicaoException">Lançada quando ocorre erro ao salvar o token</exception>
        public async Task SalvarTokenRedefinicaoAsync(int usuarioId, string token, DateTime validade)
        {
            try
            {
                if (usuarioId <= 0)
                {
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));
                }
                await _usuarioSqlAdapter.SalvarTokenRedefinicaoAsync(usuarioId, token, validade);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar token usuário {UsuarioId}", usuarioId);
                throw new SalvarTokenRedefinicaoException(usuarioId, ex);
            }
        }

        /// <summary>
        /// Atualiza a senha de um usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="senhaHash">Nova senha hash</param>
        /// <exception cref="ArgumentException">Lançada quando o ID do usuário ou senha são inválidos</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task AtualizarSenhaAsync(int usuarioId, string senhaHash)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));
                if (string.IsNullOrWhiteSpace(senhaHash))
                    throw new ArgumentException("Senha não pode ser vazia", nameof(senhaHash));

                string senhaMD5 = Recursos.ObterHashMD5(senhaHash).ToUpper();

                await _usuarioSqlAdapter.AtualizarSenhaAsync(usuarioId, senhaMD5);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a senha");
                throw new OperacaoUsuarioException("autualização da senha", ex);
            }
        }

        /// <summary>
        /// Limpa o token de recuperação de senha de um usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <exception cref="ArgumentException">Lançada quando o ID do usuário é inválido</exception>
        /// <exception cref="OperacaoUsuarioException">Lançada quando ocorre um erro durante a operação</exception>
        public async Task LimparTokenRecuperacaoAsync(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));

                await _usuarioSqlAdapter.LimparTokenRecuperacaoAsync(usuarioId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao limpar token");
                throw new OperacaoUsuarioException("limpar token", ex);
            }
        }

        #endregion

        #region 🔽 Validações

        /// <summary>
        /// Valida os dados básicos de um usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser validado</param>
        /// <returns>Lista de erros de validação</returns>
        private List<string> ValidarUsuario(Usuario usuario)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                erros.Add("Nome é obrigatório");
            else if (usuario.Nome.Length > 100)
                erros.Add("Nome não pode exceder 100 caracteres");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                erros.Add("Email é obrigatório");
            else if (!IsValidEmail(usuario.Email))
                erros.Add("Email em formato inválido");

            if (string.IsNullOrWhiteSpace(usuario.SenhaHash))
                erros.Add("Senha é obrigatória");
            else if (usuario.SenhaHash.Length < 8)
                erros.Add("Senha deve ter pelo menos 8 caracteres");

            if (usuario.PerfilId <= 0)
                erros.Add("PerfilId é obrigatório");

            return erros;
        }

        /// <summary>
        /// Valida as permissões do usuário logado
        /// </summary>
        /// <param name="usuarioLogadoId">ID do usuário logado</param>
        /// <exception cref="UnauthorizedAccessException">Lançada quando o usuário não tem permissão</exception>
        private async Task ValidarPermissoes(int usuarioLogadoId)
        {
            var usuarioLogado = await _usuarioSqlAdapter.ObterUsuarioPorIdAsync(usuarioLogadoId);
            if (usuarioLogado?.PerfilId != 1) // Supondo que 1 é Admin
            {
                throw new UnauthorizedAccessException("Apenas administradores podem cadastrar usuários");
            }
        }

        /// <summary>
        /// Valida as regras de negócio para criação de usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser validado</param>
        /// <exception cref="EmailEmUsoException">Lançada quando o email já está em uso</exception>
        /// <exception cref="PerfilNaoEncontradoException">Lançada quando o perfil não existe</exception>
        private async Task ValidarRegrasNegocio(Usuario usuario)
        {
            // Verificar se email já existe
            if (await _usuarioSqlAdapter.ObterPorEmailAsync(usuario.Email) != null)
            {
                throw new EmailEmUsoException(usuario.Email);
            }

            // Verificar se perfil existe
            if (await _perfilSqlAdapter.ObterPerfilPorIdAsync(usuario.PerfilId) == null)
            {
                throw new PerfilNaoEncontradoException(usuario.PerfilId);
            }
        }

        /// <summary>
        /// Valida as regras de negócio para atualização de usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser validado</param>
        /// <exception cref="EmailEmUsoException">Lançada quando o email já está em uso</exception>
        /// <exception cref="PerfilNaoEncontradoException">Lançada quando o perfil não existe</exception>
        private async Task ValidarRegrasNegocioUpdate(Usuario usuario)
        {
            // Verificar se email já existe
            if (await _usuarioSqlAdapter.ObterPorEmailUpdateAsync(usuario.Email, usuario.UsuarioId) != null)
            {
                throw new EmailEmUsoException(usuario.Email);
            }

            // Verificar se perfil existe
            if (await _perfilSqlAdapter.ObterPerfilPorIdAsync(usuario.PerfilId) == null)
            {
                throw new PerfilNaoEncontradoException(usuario.PerfilId);
            }
        }

        /// <summary>
        /// Prepara os dados do usuário para criação
        /// </summary>
        /// <param name="usuario">Usuário com dados originais</param>
        /// <param name="usuarioLogadoId">ID do usuário logado</param>
        /// <returns>Usuário preparado para criação</returns>
        private Usuario PrepararCriarUsuario(Usuario usuario, int usuarioLogadoId)
        {
            return new Usuario
            {
                Nome = usuario.Nome.Trim(),
                Email = usuario.Email.Trim().ToLower(),
                SenhaHash = Recursos.ObterHashMD5(usuario.SenhaHash).ToUpper(),
                PerfilId = usuario.PerfilId,
                Ativo = true,
                UsuarioInclusaoId = usuarioLogadoId,
                DataInclusao = DateTime.Now,
                NaturezaOperacao = "I",
                UsuarioOperacaoId = usuarioLogadoId,
                DataOperacao = DateTime.Now
                // Outros campos conforme necessário
            };
        }

        /// <summary>
        /// Prepara os dados do usuário para atualização
        /// </summary>
        /// <param name="usuario">Usuário com dados atualizados</param>
        /// <param name="usuarioLogadoId">ID do usuário logado</param>
        /// <returns>Usuário preparado para atualização</returns>
        private Usuario PrepararUpdateUsuario(Usuario usuario, int usuarioLogadoId)
        {
            return new Usuario
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome.Trim(),
                Email = usuario.Email.Trim().ToLower(),
                SenhaHash = Recursos.ObterHashMD5(usuario.SenhaHash).ToUpper(),
                PerfilId = usuario.PerfilId,
                Ativo = true,
                NaturezaOperacao = "A",
                UsuarioOperacaoId = usuarioLogadoId,
                DataOperacao = DateTime.Now
                // Outros campos conforme necessário
            };
        }

        /// <summary>
        /// Valida o formato de um email
        /// </summary>
        /// <param name="email">Email a ser validado</param>
        /// <returns>True se o email é válido, false caso contrário</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}