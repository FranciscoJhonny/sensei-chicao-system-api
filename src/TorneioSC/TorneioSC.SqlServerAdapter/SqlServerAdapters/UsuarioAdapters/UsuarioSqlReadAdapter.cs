using Dapper; // Adicione esta using se estiver usando Dapper
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionUsuario; // Supondo que Usuario e Perfil estão aqui

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.UsuarioAdapters
{
    internal class UsuarioSqlReadAdapter : IUsuarioSqlReadAdapter
    {
        //private readonly TorneioDbContext _dbContext;
        private readonly SqlConnection _connection; // Adicionei esta linha
        private readonly ILogger<UsuarioSqlReadAdapter> _logger;

        public UsuarioSqlReadAdapter(//TorneioDbContext dbContext,
            ILoggerFactory loggerFactory, 
            SqlAdapterContext context)
        {
            //_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<UsuarioSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            // Verificação adicional se necessário
            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
            }
        }

        public async Task<IEnumerable<Usuario>> ObterUsuarioAsync()
        {
            try
            {
                const string sql = @"SELECT * FROM dbo.Usuario u JOIN dbo.Perfil p ON p.PerfilId = u.PerfilId WHERE u.UsuarioId != 1;";

                var usuarios = await _connection.QueryAsync<Usuario, Perfil, Usuario>(sql, (usuario, perfil) =>
                {
                    usuario.Perfil = perfil ?? new Perfil();
                    return usuario;
                }, splitOn: "PerfilId");

                return usuarios ?? Enumerable.Empty<Usuario>();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter usuários");
                throw new OperacaoUsuarioException("consulta", ex);
            }
        }
        public async Task<Usuario?> ObterPorUsuarioSenhaAsync(string login, string senha)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login não pode ser vazio", nameof(login));

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia", nameof(senha));

            try
            {
                const string sql = @"SELECT UsuarioId, Nome, Email, PerfilId, Ativo, 
                            SenhaHash, TokenRecuperacaoSenha, DataRecuperacaoSenha 
                     FROM Usuario
                     WHERE Email = @login
                     AND SenhaHash = @senha
                     AND Ativo = 1";

                var usuario = await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { login, senha });

                if (usuario == null)
                    throw new CredenciaisInvalidasException();

                return usuario;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao validar credenciais");
                throw new OperacaoUsuarioException("autenticação", ex);
            }
        }
        public async Task<int> PostUsuarioAsync(Usuario usuario)
        {
            try
            {
                // Verifica se email já existe
                var usuarioExistente = await ObterPorEmailAsync(usuario.Email);
                if (usuarioExistente != null)
                    throw new EmailEmUsoException(usuario.Email);

                const string sql = @"INSERT INTO Usuario (Nome, Email, SenhaHash, PerfilId,Ativo,UsuarioInclusaoId, DataInclusao,NaturezaOperacao,UsuarioOperacaoId, DataOperacao)
                          VALUES (@Nome, @Email, @SenhaHash, @PerfilId, @Ativo,@UsuarioInclusaoId,@DataInclusao,@NaturezaOperacao,@UsuarioOperacaoId, @DataOperacao);
                          SELECT CAST(SCOPE_IDENTITY() as int)";

                var parametros = new
                {
                    usuario.Nome,
                    usuario.Email,
                    usuario.SenhaHash,
                    usuario.PerfilId,
                    usuario.Ativo,
                    usuario.UsuarioInclusaoId,
                    usuario.DataInclusao,
                    usuario.NaturezaOperacao,
                    usuario.UsuarioOperacaoId,
                    usuario.DataOperacao
                };

                return await _connection.ExecuteScalarAsync<int>(sql, parametros);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                throw new OperacaoUsuarioException("criação", ex);
            }
        }
        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            try
            {
                const string sql = "SELECT * FROM Usuario WHERE Email = @email";
                return await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { email });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por email: {email}");
                throw new OperacaoUsuarioException("busca por email", ex);
            }
        }
        public async Task<Usuario?> ObterPorEmailUpdateAsync(string email, int usuarioId)
        {
            try
            {
                const string sql = "SELECT * FROM Usuario WHERE Email = @email and UsuarioID != @usuarioId";
                return await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { email, usuarioId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por email: {email}");
                throw new OperacaoUsuarioException("busca por email", ex);
            }
        }
        public async Task<Usuario?> ObterUsuarioPorIdAsync(int usuarioId)
        {
            try
            {
                const string sql = @"SELECT * FROM dbo.Usuario u JOIN dbo.Perfil p ON p.PerfilId = u.PerfilId WHERE u.UsuarioId = @usuarioId;";

                var usuarios = await _connection.QueryAsync<Usuario, Perfil, Usuario>(sql, (usuario, perfil) =>
                {
                    usuario.Perfil = perfil ?? new Perfil();
                    return usuario;
                }, new { usuarioId }, splitOn: "PerfilId");

                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                    throw new UsuarioNaoEncontradoException(usuarioId);

                return usuario;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por ID: {usuarioId}");
                throw new OperacaoUsuarioException("busca por ID", ex);
            }
        }
        public async Task<int> UpdateUsuario(Usuario usuario)
        {
            try
            {
                // Validação básica
                if (usuario == null)
                    throw new ArgumentNullException(nameof(usuario));

                if (usuario.UsuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuario.UsuarioId));

                const string sql = @"UPDATE [dbo].[Usuario]
                           SET [Nome] = @Nome
                              ,[Email] = @Email
                              ,[PerfilId] = @PerfilId
                              ,[Ativo] = @Ativo      
                              ,[DataOperacao] = GETDATE()
                              ,[SenhaHash] = @SenhaHash
                         WHERE UsuarioId = @UsuarioId";

                var rowsAffected = await _connection.ExecuteAsync(sql, usuario);

                if (rowsAffected == 0)
                    throw new UsuarioNaoEncontradoException(usuario.UsuarioId);

                return rowsAffected;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário {UsuarioId}", usuario?.UsuarioId);
                throw new AtualizacaoUsuarioException(usuario?.UsuarioId ?? 0, ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar usuário {UsuarioId}", usuario?.UsuarioId);
                throw new AtualizacaoUsuarioException(usuario?.UsuarioId ?? 0, ex);
            }
        }
        public async Task<bool> DeleteUsuarioPorIdAsync(int usuarioId)
        {
            try
            {
                // Validação básica
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));

                const string sql = @"UPDATE [dbo].[Usuario]
                           SET [Ativo] = 0,
                               [DataOperacao] = GETDATE()
                         WHERE UsuarioId = @usuarioId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new { usuarioId });

                if (rowsAffected == 0)
                    throw new UsuarioNaoEncontradoException(usuarioId);

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao inativar usuário {UsuarioId}", usuarioId);
                throw new ExclusaoUsuarioException(usuarioId, ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao inativar usuário {UsuarioId}", usuarioId);
                throw new ExclusaoUsuarioException(usuarioId, ex);
            }
        }
        public async Task<int> VerificaUsuarioAsync(string email)
        {
            try
            {
                const string sql = "SELECT * FROM Usuario WHERE Email = @email";
                return await _connection.ExecuteScalarAsync<int>(sql, new { email }, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por email: {email}");
                throw new OperacaoUsuarioException("verificação do email", ex);
            }
        }

        public async Task SalvarTokenRedefinicaoAsync(int usuarioId, string token, DateTime validade)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));

                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Token não pode ser vazia", nameof(token));

                const string sql = @"UPDATE Usuario SET TokenRecuperacaoSenha = @token, DataRecuperacaoSenha = @validade WHERE UsuarioId = @usuarioId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new { usuarioId, token, validade });

                if (rowsAffected == 0)
                    throw new UsuarioNaoEncontradoException(usuarioId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao salvar token usuário {UsuarioId}", usuarioId);
                throw new SalvarTokenRedefinicaoException(usuarioId, ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado salvar token usuário {UsuarioId}", usuarioId);
                throw new SalvarTokenRedefinicaoException(usuarioId, ex);
            }

        }
        public async Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Token não pode ser vazia", nameof(token));

                const string sql = @"SELECT * FROM Usuario WHERE TokenRecuperacaoSenha = @token";

                var usuario = await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { token });

                return usuario;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário com o token: {token}");
                throw new OperacaoUsuarioException("busca por ID", ex);
            }
        }

        public async Task AtualizarSenhaAsync(int usuarioId, string senhaHash)
        {

            try
            {
                if (usuarioId <= 0)
                    throw new ArgumentException("ID do usuário inválido", nameof(usuarioId));
                if (string.IsNullOrWhiteSpace(senhaHash))
                    throw new ArgumentException("Senha não pode ser vazia", nameof(senhaHash));

                const string sql = @"UPDATE Usuario SET SenhaHash = @senhaHash WHERE UsuarioId = @usuarioId";

                await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { usuarioId, senhaHash });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao validar credenciais");
                throw new OperacaoUsuarioException("autenticação", ex);
            }
        }

        public async Task LimparTokenRecuperacaoAsync(int usuarioId)
        {
            try
            {
                const string sql = "UPDATE Usuario SET TokenRecuperacaoSenha = NULL, DataRecuperacaoSenha = NULL WHERE UsuarioId = @usuarioId";
                
                await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { usuarioId });
                

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar usuário por ID: {usuarioId}");
                throw new OperacaoUsuarioException("busca por ID", ex);
            }
        }


    }
}