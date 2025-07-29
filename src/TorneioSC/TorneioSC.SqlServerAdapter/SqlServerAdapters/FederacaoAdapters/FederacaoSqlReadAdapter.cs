using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.FederacaoAdapters
{
    internal class FederacaoSqlReadAdapter : IFederacaoSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<FederacaoSqlReadAdapter> _logger;

        public FederacaoSqlReadAdapter(
            ILoggerFactory loggerFactory,
            SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<FederacaoSqlReadAdapter>() ??
                     throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        public async Task<IEnumerable<Federacao>> ObterFederacaoAsync()
        {
            try
            {
                const string sql = @"SELECT 
                                f.*, 
                                e.*, 
                                t.*, 
                                tt.* 
                            FROM dbo.Federacao f
                            LEFT JOIN dbo.FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                            LEFT JOIN dbo.Endereco e ON e.EnderecoId = fe.EnderecoId
                            LEFT JOIN dbo.FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                            LEFT JOIN dbo.Telefone t ON t.TelefoneId = ft.TelefoneId
                            LEFT JOIN dbo.TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                            WHERE f.Ativo = 1";

                var federacoes = new Dictionary<int, Federacao>();

                await _connection.QueryAsync<Federacao, Endereco, Telefone, TipoTelefone, Federacao>(
                    sql,
                    (federacao, endereco, telefone, tipoTelefone) =>
                    {
                        if (!federacoes.TryGetValue(federacao.FederacaoId, out var federacaoEntry))
                        {
                            federacaoEntry = federacao;
                            federacaoEntry.Enderecos = new List<Endereco>();
                            federacaoEntry.Telefones = new List<Telefone>();
                            federacoes.Add(federacaoEntry.FederacaoId, federacaoEntry);
                        }

                        if (endereco != null && !federacaoEntry.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                        {
                            federacaoEntry.Enderecos.Add(endereco);
                        }

                        if (telefone != null && tipoTelefone != null)
                        {
                            telefone.TipoTelefone = tipoTelefone;
                            if (!federacaoEntry.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                            {
                                federacaoEntry.Telefones.Add(telefone);
                            }
                        }

                        return federacaoEntry;
                    },
                    splitOn: "EnderecoId,TelefoneId,TipoTelefoneId");

                return federacoes.Values;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter federações completas");
                throw new OperacaoFederacaoException("consulta completa", ex);
            }
        }

        public async Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId)
        {
            try
            {
                const string sql = @"
                                    SELECT 
                                        f.*, 
                                        e.*, 
                                        t.*, 
                                        tt.*
                                    FROM dbo.Federacao f
                                    LEFT JOIN dbo.FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                                    LEFT JOIN dbo.Endereco e ON e.EnderecoId = fe.EnderecoId
                                    LEFT JOIN dbo.FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                                    LEFT JOIN dbo.Telefone t ON t.TelefoneId = ft.TelefoneId
                                    LEFT JOIN dbo.TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                                    WHERE f.FederacaoId = @federacaoId";

                Federacao? federacao = null;

                await _connection.QueryAsync<Federacao, Endereco, Telefone, TipoTelefone, Federacao>(
                    sql,
                    (f, endereco, telefone, tipoTelefone) =>
                    {
                        if (federacao == null)
                        {
                            federacao = f;
                            federacao.Enderecos = new List<Endereco>();
                            federacao.Telefones = new List<Telefone>();
                        }

                        if (endereco != null && !federacao.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                        {
                            federacao.Enderecos.Add(endereco);
                        }

                        if (telefone != null)
                        {
                            telefone.TipoTelefone = tipoTelefone;
                            if (!federacao.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                            {
                                federacao.Telefones.Add(telefone);
                            }
                        }

                        return f;
                    },
                    new { federacaoId },
                    splitOn: "EnderecoId,TelefoneId,TipoTelefoneId");

                if (federacao == null)
                    throw new FederacaoNaoEncontradaException(federacaoId);

                return federacao;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar federação por ID: {federacaoId}");
                throw new OperacaoFederacaoException("busca por ID", ex);
            }
        }


        public async Task<Federacao?> ObterPorCnpjAsync(string cnpj)
        {
            try
            {
                const string sql = "SELECT * FROM Federacao WHERE CNPJ = @cnpj";
                var federacao = await _connection.QueryFirstOrDefaultAsync<Federacao>(sql, new { cnpj });

                if (federacao != null)
                    throw new CnpjEmUsoException(cnpj);
                return federacao;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar federação por CNPJ: {cnpj}");
                throw new OperacaoFederacaoException("busca por CNPJ", ex);
            }
        }

        public async Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId)
        {
            try
            {
                const string sql = "SELECT * FROM Federacao WHERE CNPJ = @cnpj AND FederacaoId != @federacaoId";
                return await _connection.QueryFirstOrDefaultAsync<Federacao>(sql, new { cnpj, federacaoId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar federação por CNPJ: {cnpj}");
                throw new OperacaoFederacaoException("busca por CNPJ", ex);
            }
        }

        public async Task<int> PostFederacaoAsync(Federacao federacao)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {

                // 1. Inserir a federação
                const string sqlFederacao = @"
                                                INSERT INTO Federacao 
                                                    (Nome, MunicipioId, CNPJ, Email, Site, DataFundacao, Portaria, 
                                                     Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                     UsuarioOperacaoId, DataOperacao)
                                                VALUES 
                                                    (@Nome, @MunicipioId, @CNPJ, @Email, @Site, @DataFundacao, @Portaria, 
                                                     @Ativo, @UsuarioInclusaoId, @DataInclusao, @NaturezaOperacao, 
                                                     @UsuarioOperacaoId, @DataOperacao);
                                                SELECT CAST(SCOPE_IDENTITY() as int);";

                int federacaoId = await _connection.ExecuteScalarAsync<int>(sqlFederacao, federacao, transaction);

                // 2. Inserir endereços (e relacionamento)
                foreach (var endereco in federacao.Enderecos)
                {
                    const string sqlEndereco = @"
                                            INSERT INTO Endereco 
                                                (Logradouro, Numero, Complemento, Cep, Bairro, Ativo, UsuarioInclusaoId, 
                                                 DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                            VALUES 
                                                (@Logradouro, @Numero, @Complemento, @Cep, @Bairro, @Ativo, @UsuarioInclusaoId, 
                                                 @DataInclusao, @NaturezaOperacao, @UsuarioOperacaoId, @DataOperacao);
                                            SELECT CAST(SCOPE_IDENTITY() as int);";

                    int enderecoId = await _connection.ExecuteScalarAsync<int>(sqlEndereco, endereco, transaction);

                    const string sqlRelEndereco = @"INSERT INTO FederacaoEndereco (FederacaoId, EnderecoId, Ativo, UsuarioInclusaoId, 
                                                 DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao) 
                                                   VALUES (@FederacaoId, @EnderecoId, @Ativo, @UsuarioInclusaoId, 
                                                 @DataInclusao, @NaturezaOperacao, @UsuarioOperacaoId, @DataOperacao);";
                    await _connection.ExecuteAsync(sqlRelEndereco, new
                    {
                        FederacaoId = federacaoId,
                        EnderecoId = enderecoId,
                        Ativo = endereco.Ativo,
                        UsuarioInclusaoId = endereco.UsuarioInclusaoId,
                        DataInclusao = endereco.DataInclusao,
                        NaturezaOperacao = endereco.NaturezaOperacao,
                        UsuarioOperacaoId = endereco.UsuarioOperacaoId,
                        DataOperacao = endereco.DataOperacao
                    }, transaction);
                }

                // 3. Inserir telefones (e relacionamento)
                foreach (var telefone in federacao.Telefones)
                {
                    const string sqlTelefone = @"
                                                INSERT INTO Telefone 
                                                    (NumeroTelefone, TipoTelefoneId, Ativo, UsuarioInclusaoId, DataInclusao, 
                                                     NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                VALUES 
                                                    (@NumeroTelefone, @TipoTelefoneId, @Ativo, @UsuarioInclusaoId, @DataInclusao, 
                                                     @NaturezaOperacao, @UsuarioOperacaoId, @DataOperacao);
                                                SELECT CAST(SCOPE_IDENTITY() as int);";

                    int telefoneId = await _connection.ExecuteScalarAsync<int>(sqlTelefone, telefone, transaction);

                    const string sqlRelTelefone = @"INSERT INTO FederacaoTelefone (FederacaoId, TelefoneId, Ativo, UsuarioInclusaoId, DataInclusao, 
                                                     NaturezaOperacao, UsuarioOperacaoId, DataOperacao) VALUES (@FederacaoId, @TelefoneId, @Ativo, @UsuarioInclusaoId, @DataInclusao, 
                                                     @NaturezaOperacao, @UsuarioOperacaoId, @DataOperacao);";
                    await _connection.ExecuteAsync(sqlRelTelefone, new
                    {
                        FederacaoId = federacaoId,
                        TelefoneId = telefoneId,
                        Ativo = telefone.Ativo,
                        UsuarioInclusaoId = telefone.UsuarioInclusaoId,
                        DataInclusao = telefone.DataInclusao,
                        NaturezaOperacao = telefone.NaturezaOperacao,
                        UsuarioOperacaoId = telefone.UsuarioOperacaoId,
                        DataOperacao = telefone.DataOperacao
                    }, transaction);
                }

                // 4. Commit
                transaction.Commit();
                return federacaoId;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao criar federação");
                throw new OperacaoFederacaoException("criação", ex);
            }
        }




        public async Task<int> UpdateFederacaoAsync(Federacao federacao)
        {
            try
            {
                if (federacao == null)
                    throw new ArgumentNullException(nameof(federacao));

                if (federacao.FederacaoId <= 0)
                    throw new ArgumentException("ID da federação inválido", nameof(federacao.FederacaoId));

                const string sql = @"UPDATE Federacao
                           SET Nome = @Nome,
                               MunicipioId = @MunicipioId,
                               CNPJ = @CNPJ,
                               Email = @Email,
                               Site = @Site,
                               DataFundacao = @DataFundacao,
                               Portaria = @Portaria,
                               Ativo = @Ativo,
                               DataOperacao = GETDATE(),
                               UsuarioOperacaoId = @UsuarioOperacaoId,
                               NaturezaOperacao = @NaturezaOperacao
                         WHERE FederacaoId = @FederacaoId";

                var rowsAffected = await _connection.ExecuteAsync(sql, federacao);

                if (rowsAffected == 0)
                    throw new FederacaoNaoEncontradaException(federacao.FederacaoId);

                return rowsAffected;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar federação {FederacaoId}", federacao?.FederacaoId);
                throw new AtualizacaoFederacaoException(federacao?.FederacaoId ?? 0, ex);
            }
        }

        public async Task<bool> DeleteFederacaoPorIdAsync(int federacaoId)
        {
            try
            {
                if (federacaoId <= 0)
                    throw new ArgumentException("ID da federação inválido", nameof(federacaoId));

                const string sql = @"UPDATE Federacao
                           SET Ativo = 0,
                               DataOperacao = GETDATE()
                         WHERE FederacaoId = @federacaoId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new { federacaoId });

                if (rowsAffected == 0)
                    throw new FederacaoNaoEncontradaException(federacaoId);

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao inativar federação {FederacaoId}", federacaoId);
                throw new ExclusaoFederacaoException(federacaoId, ex);
            }
        }


        public async Task<int> VincularEnderecoAsync(int federacaoId, int enderecoId, int? usuarioId)
        {
            try
            {
                const string sql = @"INSERT INTO FederacaoEndereco 
                          (FederacaoId, EnderecoId, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao)
                          VALUES 
                          (@federacaoId, @enderecoId, 1, @usuarioId, GETDATE(), 'I');
                          SELECT CAST(SCOPE_IDENTITY() as int)";

                return await _connection.ExecuteScalarAsync<int>(sql, new { federacaoId, enderecoId, usuarioId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao vincular endereço à federação {FederacaoId}", federacaoId);
                throw new VinculoEnderecoException(federacaoId, ex);
            }
        }

        public async Task<int> VincularTelefoneAsync(int federacaoId, int telefoneId, int? usuarioId)
        {
            try
            {
                const string sql = @"INSERT INTO FederacaoTelefone 
                          (FederacaoId, TelefoneId, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao)
                          VALUES 
                          (@federacaoId, @telefoneId, 1, @usuarioId, GETDATE(), 'I');
                          SELECT CAST(SCOPE_IDENTITY() as int)";

                return await _connection.ExecuteScalarAsync<int>(sql, new { federacaoId, telefoneId, usuarioId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao vincular telefone à federação {FederacaoId}", federacaoId);
                throw new VinculoTelefoneException(federacaoId, ex);
            }
        }
    }
}