using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.FederacaoAdapters
{
    internal class FederacaoSqlWriteAdapter : IFederacaoSqlWriteAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<FederacaoSqlWriteAdapter> _logger;

        public FederacaoSqlWriteAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<FederacaoSqlWriteAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        public async Task<int> PostFederacaoAsync(Federacao federacao)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {
                // 1. Inserir a federação
                const string sqlFederacao = @"INSERT INTO Federacao 
                                                  (Nome, MunicipioId, CNPJ, Email, Site, DataFundacao, Portaria, 
                                                   Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                   UsuarioOperacaoId, DataOperacao)
                                              VALUES 
                                                  (@Nome, @MunicipioId, @CNPJ, @Email, @Site, @DataFundacao, @Portaria, 
                                                   @Ativo, @UsuarioInclusaoId, GETDATE(), 'I', 
                                                   @UsuarioOperacaoId, GETDATE());
                                              SELECT CAST(SCOPE_IDENTITY() AS int);";

                int federacaoId = await _connection.ExecuteScalarAsync<int>(sqlFederacao, federacao, transaction);

                // 2. Sincronizar Endereços
                await SincronizarEnderecosAsync(federacaoId, federacao.Enderecos, federacao.UsuarioOperacaoId, transaction);

                // 3. Sincronizar Telefones
                await SincronizarTelefonesAsync(federacaoId, federacao.Telefones, federacao.UsuarioOperacaoId, transaction);

                // 4. Sincronizar Redes Sociais (opcional, se existir)
                if (federacao.FederacaoRedeSociais != null)
                {
                    await SincronizarRedesSociaisAsync(federacaoId, federacao.FederacaoRedeSociais, federacao.UsuarioOperacaoId, transaction);
                }

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

        public async Task<int> PutFederacaoAsync(Federacao federacao)
        {
            if (federacao == null)
                throw new ArgumentNullException(nameof(federacao));

            if (federacao.FederacaoId <= 0)
                throw new ArgumentException("ID da federação inválido", nameof(federacao.FederacaoId));

            using var transaction = _connection.BeginTransaction();

            try
            {
                // 1. Atualiza a federação
                const string sqlFederacao = @" UPDATE Federacao
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

                var rowsAffected = await _connection.ExecuteAsync(sqlFederacao, federacao, transaction);

                if (rowsAffected == 0)
                    throw new FederacaoNaoEncontradaException(federacao.FederacaoId);

                // 2. Sincronizar Endereços
                await SincronizarEnderecosAsync(federacao.FederacaoId, federacao.Enderecos, federacao.UsuarioOperacaoId, transaction);

                // 3. Sincronizar Telefones
                await SincronizarTelefonesAsync(federacao.FederacaoId, federacao.Telefones, federacao.UsuarioOperacaoId, transaction);

                // 4. Sincronizar Redes Sociais
                if (federacao.FederacaoRedeSociais != null)
                {
                    await SincronizarRedesSociaisAsync(federacao.FederacaoId, federacao.FederacaoRedeSociais, federacao.UsuarioOperacaoId, transaction);
                }

                transaction.Commit();
                return rowsAffected;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao atualizar federação {FederacaoId}", federacao?.FederacaoId);
                throw new AtualizacaoFederacaoException(federacao?.FederacaoId ?? 0, ex);
            }
        }

        public async Task<bool> InativarFederacaoPorIdAsync(int federacaoId, int usuarioOperacaoId)
        {
            try
            {
                if (federacaoId <= 0)
                    throw new ArgumentException("ID da federação inválido", nameof(federacaoId));

                if (usuarioOperacaoId <= 0)
                    throw new ArgumentException("ID do usuário operador inválido", nameof(usuarioOperacaoId));

                const string sql = @" UPDATE Federacao
                                      SET Ativo = 0,
                                          DataOperacao = GETDATE(),
                                          UsuarioOperacaoId = @usuarioOperacaoId,
                                          NaturezaOperacao = 'A'
                                      WHERE FederacaoId = @federacaoId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new
                {
                    federacaoId,
                    usuarioOperacaoId
                });

                if (rowsAffected == 0)
                    throw new FederacaoNaoEncontradaException(federacaoId);

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao inativar federação {FederacaoId} pelo usuário {UsuarioOperacaoId}", federacaoId, usuarioOperacaoId);
                throw new ExclusaoFederacaoException(federacaoId, ex);
            }
        }

        //📍 Sincronizar Endereços
        private async Task SincronizarEnderecosAsync(int federacaoId, IEnumerable<Endereco> novosEnderecos, int usuarioId, IDbTransaction transaction)
        {
            const string sqlAtuais = @" SELECT e.EnderecoId
                                        FROM Endereco e
                                        INNER JOIN FederacaoEndereco fe ON fe.EnderecoId = e.EnderecoId
                                        WHERE fe.FederacaoId = @FederacaoId AND fe.Ativo = 1";

            var atuais = (await _connection.QueryAsync<int>(sqlAtuais, new { FederacaoId = federacaoId }, transaction)).ToHashSet();

            var recebidos = new HashSet<int>();
            var updates = new List<Endereco>();

            foreach (var endereco in novosEnderecos)
            {
                if (endereco.EnderecoId > 0)
                {
                    if (atuais.Contains(endereco.EnderecoId))
                    {
                        recebidos.Add(endereco.EnderecoId);
                        updates.Add(endereco);
                    }
                }
                else
                {
                    const string insertEndereco = @" INSERT INTO Endereco (Logradouro, Numero, Complemento, Cep, Bairro, Ativo, 
                                                                           UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                                           UsuarioOperacaoId, DataOperacao)
                                                     VALUES (@Logradouro, @Numero, @Complemento, @Cep, @Bairro, @Ativo, 
                                                             @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());
                                                     SELECT CAST(SCOPE_IDENTITY() AS int);";

                    var novoId = await _connection.QuerySingleAsync<int>(insertEndereco, new
                    {
                        endereco.Logradouro,
                        endereco.Numero,
                        endereco.Complemento,
                        endereco.Cep,
                        endereco.Bairro,
                        Ativo = true,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);

                    const string insertVinculo = @"
                                                    INSERT INTO FederacaoEndereco (FederacaoId, EnderecoId, Ativo, 
                                                                                  UsuarioInclusaoId, DataInclusao, 
                                                                                  NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@FederacaoId, @EnderecoId, 1, @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insertVinculo, new
                    {
                        FederacaoId = federacaoId,
                        EnderecoId = novoId,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);
                }
            }

            foreach (var endereco in updates)
            {
                const string updateEndereco = @"
                                                UPDATE Endereco SET
                                                    Logradouro = @Logradouro,
                                                    Numero = @Numero,
                                                    Complemento = @Complemento,
                                                    Cep = @Cep,
                                                    Bairro = @Bairro,
                                                    Ativo = @Ativo,
                                                    UsuarioOperacaoId = @UsuarioOperacaoId,
                                                    DataOperacao = GETDATE()
                                                WHERE EnderecoId = @EnderecoId";

                await _connection.ExecuteAsync(updateEndereco, new
                {
                    endereco.Logradouro,
                    endereco.Numero,
                    endereco.Complemento,
                    endereco.Cep,
                    endereco.Bairro,
                    endereco.Ativo,
                    UsuarioOperacaoId = usuarioId,
                    endereco.EnderecoId
                }, transaction);
            }

            var paraExcluir = atuais.Except(recebidos);
            foreach (var id in paraExcluir)
            {
                await _connection.ExecuteAsync("DELETE FROM FederacaoEndereco WHERE FederacaoId = @FederacaoId AND EnderecoId = @EnderecoId",
                    new { FederacaoId = federacaoId, EnderecoId = id }, transaction);

                await _connection.ExecuteAsync("DELETE FROM Endereco WHERE EnderecoId = @EnderecoId",
                    new { EnderecoId = id }, transaction);
            }
        }

        //📞 Sincronizar Telefones
        private async Task SincronizarTelefonesAsync(int federacaoId, IEnumerable<Telefone> novosTelefones, int usuarioId, IDbTransaction transaction)
        {
            const string sqlAtuais = @"
                                        SELECT t.TelefoneId
                                        FROM Telefone t
                                        INNER JOIN FederacaoTelefone ft ON ft.TelefoneId = t.TelefoneId
                                        WHERE ft.FederacaoId = @FederacaoId AND ft.Ativo = 1";

            var atuais = (await _connection.QueryAsync<int>(sqlAtuais, new { FederacaoId = federacaoId }, transaction)).ToHashSet();

            var recebidos = new HashSet<int>();
            var updates = new List<Telefone>();

            foreach (var tel in novosTelefones)
            {
                if (tel.TelefoneId > 0)
                {
                    if (atuais.Contains(tel.TelefoneId))
                    {
                        recebidos.Add(tel.TelefoneId);
                        updates.Add(tel);
                    }
                }
                else
                {
                    const string insertTelefone = @"
                                                    INSERT INTO Telefone (NumeroTelefone, TipoTelefoneId, Ativo, 
                                                                          UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                                          UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@NumeroTelefone, @TipoTelefoneId, @Ativo, 
                                                            @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());
                                                    SELECT CAST(SCOPE_IDENTITY() AS int);";

                    var novoId = await _connection.QuerySingleAsync<int>(insertTelefone, new
                    {
                        tel.NumeroTelefone,
                        tel.TipoTelefoneId,
                        Ativo = true,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);

                    const string insertVinculo = @"
                                                    INSERT INTO FederacaoTelefone (FederacaoId, TelefoneId, Ativo, 
                                                                                  UsuarioInclusaoId, DataInclusao, 
                                                                                  NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@FederacaoId, @TelefoneId, 1, @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insertVinculo, new
                    {
                        FederacaoId = federacaoId,
                        TelefoneId = novoId,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);
                }
            }

            foreach (var tel in updates)
            {
                const string updateTelefone = @"
                                                UPDATE Telefone SET
                                                    NumeroTelefone = @NumeroTelefone,
                                                    TipoTelefoneId = @TipoTelefoneId,
                                                    Ativo = @Ativo,
                                                    UsuarioOperacaoId = @UsuarioOperacaoId,
                                                    DataOperacao = GETDATE()
                                                WHERE TelefoneId = @TelefoneId";

                await _connection.ExecuteAsync(updateTelefone, new
                {
                    tel.NumeroTelefone,
                    tel.TipoTelefoneId,
                    tel.Ativo,
                    UsuarioOperacaoId = usuarioId,
                    tel.TelefoneId
                }, transaction);
            }

            var paraExcluir = atuais.Except(recebidos);
            foreach (var id in paraExcluir)
            {
                await _connection.ExecuteAsync("DELETE FROM FederacaoTelefone WHERE FederacaoId = @FederacaoId AND TelefoneId = @TelefoneId",
                    new { FederacaoId = federacaoId, TelefoneId = id }, transaction);

                await _connection.ExecuteAsync("DELETE FROM Telefone WHERE TelefoneId = @TelefoneId",
                    new { TelefoneId = id }, transaction);
            }
        }

        //🌐 Sincronizar Redes Sociais
        private async Task SincronizarRedesSociaisAsync(int federacaoId, IEnumerable<FederacaoRedeSocial> novasRedes, int usuarioId, IDbTransaction transaction)
        {
            const string sqlAtuais = @"
                                      SELECT FederacaoRedeSocialId, RedeSocialId
                                      FROM FederacaoRedeSocial
                                      WHERE FederacaoId = @FederacaoId AND Ativo = 1";

            var atuais = await _connection.QueryAsync<(int IdVinculo, int RedeSocialId)>(sqlAtuais, new { FederacaoId = federacaoId }, transaction);

            var atuaisMap = atuais.ToDictionary(x => x.RedeSocialId, x => x.IdVinculo);
            var recebidos = new HashSet<int>();

            foreach (var rede in novasRedes)
            {
                if (rede.RedeSocialId <= 0) continue;

                if (atuaisMap.ContainsKey(rede.RedeSocialId))
                {
                    const string update = @"
                                            UPDATE FederacaoRedeSocial SET
                                                PerfilUrl = @PerfilUrl,
                                                UsuarioOperacaoId = @UsuarioOperacaoId,
                                                DataOperacao = GETDATE()
                                            WHERE FederacaoRedeSocialId = @FederacaoRedeSocialId";

                    await _connection.ExecuteAsync(update, new
                    {
                        PerfilUrl = rede.PerfilUrl,
                        UsuarioOperacaoId = usuarioId,
                        FederacaoRedeSocialId = atuaisMap[rede.RedeSocialId]
                    }, transaction);

                    recebidos.Add(rede.RedeSocialId);
                }
                else
                {
                    const string insert = @"
                                            INSERT INTO FederacaoRedeSocial 
                                                (FederacaoId, RedeSocialId, PerfilUrl, Ativo, 
                                                 UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                 UsuarioOperacaoId, DataOperacao)
                                            VALUES 
                                                (@FederacaoId, @RedeSocialId, @PerfilUrl, 1, 
                                                 @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insert, new
                    {
                        FederacaoId = federacaoId,
                        rede.RedeSocialId,
                        rede.PerfilUrl,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);
                }
            }

            var paraExcluir = atuais.Where(x => !recebidos.Contains(x.RedeSocialId));
            foreach (var item in paraExcluir)
            {
                await _connection.ExecuteAsync("DELETE FROM FederacaoRedeSocial WHERE FederacaoRedeSocialId = @FederacaoRedeSocialId",
                    new { FederacaoRedeSocialId = item.IdVinculo }, transaction);
            }
        }
    }
}