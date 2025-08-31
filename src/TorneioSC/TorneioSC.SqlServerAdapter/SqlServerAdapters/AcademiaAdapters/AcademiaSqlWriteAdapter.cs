using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.AcademiaAdapters
{
    internal class AcademiaSqlWriteAdapter : IAcademiaSqlWriteAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<AcademiaSqlWriteAdapter> _logger;

        public AcademiaSqlWriteAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<AcademiaSqlWriteAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }
        public async Task<int> PostAcademiaAsync(Academia academia)
        {
            if (academia == null)
                throw new ArgumentNullException(nameof(academia));

            using var transaction = _connection.BeginTransaction();

            try
            {
                // 1. Inserir a academia
                const string sqlAcademia = @"
                                                INSERT INTO dbo.Academia
                                                (Nome, FederacaoId, MunicipioId, CNPJ, ResponsavelNome, ResponsavelCpf, 
                                                 Email, LogoUrl, Descricao, Ativo, UsuarioInclusaoId, DataInclusao, 
                                                 NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                VALUES
                                                (@Nome, @FederacaoId, @MunicipioId, @CNPJ, @ResponsavelNome, @ResponsavelCpf, 
                                                 @Email, @LogoUrl, @Descricao, @Ativo, @UsuarioInclusaoId, GETDATE(), 
                                                 'I', @UsuarioOperacaoId, GETDATE());
                                                SELECT CAST(SCOPE_IDENTITY() AS int);";

                int academiaId = await _connection.ExecuteScalarAsync<int>(sqlAcademia, academia, transaction);

                // 2. Sincronizar Endereços (todos serão novos)
                await SincronizarEnderecosAsync(academiaId, academia.Enderecos, academia.UsuarioInclusaoId, transaction);

                // 3. Sincronizar Telefones (todos serão novos)
                await SincronizarTelefonesAsync(academiaId, academia.Telefones, academia.UsuarioInclusaoId, transaction);

                // 4. Sincronizar Redes Sociais (todas serão novas ou atualizações se já existirem — mas não deveriam)
                if (academia.AcademiaRedeSociais != null)
                {
                    await SincronizarRedesSociaisAsync(academiaId, academia.AcademiaRedeSociais, academia.UsuarioInclusaoId, transaction);
                }

                transaction.Commit();
                return academiaId;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao criar academia");
                throw new OperacaoAcademiaException("criação", ex);
            }
        }
        public async Task<int> PutAcademiaAsync(Academia academia)
        {
            if (academia == null)
                throw new ArgumentNullException(nameof(academia));

            if (academia.AcademiaId <= 0)
                throw new ArgumentException("ID da academia inválido", nameof(academia.AcademiaId));

            using var transaction = _connection.BeginTransaction();

            try
            {
                // 1. Atualiza a academia
                const string sqlAcademia = @"
                                            UPDATE Academia
                                            SET Nome = @Nome,
                                                FederacaoId = @FederacaoId,
                                                MunicipioId = @MunicipioId,
                                                CNPJ = @CNPJ,
                                                ResponsavelNome = @ResponsavelNome,
                                                ResponsavelCpf = @ResponsavelCpf,
                                                Email = @Email,
                                                LogoUrl = @LogoUrl,
                                                Descricao = @Descricao,
                                                Ativo = @Ativo,
                                                DataOperacao = GETDATE(),
                                                UsuarioOperacaoId = @UsuarioOperacaoId,
                                                NaturezaOperacao = @NaturezaOperacao
                                            WHERE AcademiaId = @AcademiaId";

                var rowsAffected = await _connection.ExecuteAsync(sqlAcademia, academia, transaction);

                if (rowsAffected == 0)
                    throw new AcademiaNaoEncontradaException(academia.AcademiaId);

                // 2. Sincronizar Endereços
                await SincronizarEnderecosAsync(academia.AcademiaId, academia.Enderecos, academia.UsuarioOperacaoId, transaction);

                // 3. Sincronizar Telefones
                await SincronizarTelefonesAsync(academia.AcademiaId, academia.Telefones, academia.UsuarioOperacaoId, transaction);

                // 4. Sincronizar Redes Sociais
                if (academia.AcademiaRedeSociais != null)
                {
                    await SincronizarRedesSociaisAsync(academia.AcademiaId, academia.AcademiaRedeSociais, academia.UsuarioOperacaoId, transaction);
                }

                transaction.Commit();
                return rowsAffected;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Erro ao atualizar academia {AcademiaId}", academia?.AcademiaId);
                throw new AtualizacaoAcademiaException(academia?.AcademiaId ?? 0, ex);
            }
        }

        //📍 1. Sincronizar Endereços
        private async Task SincronizarEnderecosAsync(int academiaId, IEnumerable<Endereco> novosEnderecos, int usuarioId, IDbTransaction transaction)
        {
            // Busca os endereços atuais vinculados à academia
            const string sqlAtuais = @"
                                        SELECT e.EnderecoId
                                        FROM Endereco e
                                        INNER JOIN AcademiaEndereco ae ON ae.EnderecoId = e.EnderecoId
                                        WHERE ae.AcademiaId = @AcademiaId AND ae.Ativo = 1";

            var atuais = (await _connection.QueryAsync<int>(sqlAtuais, new { AcademiaId = academiaId }, transaction)).ToHashSet();

            var recebidos = new HashSet<int>();
            var updates = new List<Endereco>();

            foreach (var endereco in novosEnderecos)
            {
                if (endereco.EnderecoId > 0)
                {
                    if (atuais.Contains(endereco.EnderecoId))
                    {
                        recebidos.Add(endereco.EnderecoId);
                        updates.Add(endereco); // Marca para atualizar
                    }
                    // Se não estiver em `atuais`, ignora (não pertence à academia)
                }
                else
                {
                    // Novo endereço
                    const string insertEndereco = @"
                                                    INSERT INTO Endereco (Logradouro, Numero, Complemento, Cep, Bairro, Ativo, 
                                                                          UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                                          UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@Logradouro, @Numero, @Complemento, @Cep, @Bairro, @Ativo, 
                                                            @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());
                                                    SELECT CAST(SCOPE_IDENTITY() as int);";

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
                                                    INSERT INTO AcademiaEndereco (AcademiaId, EnderecoId, Ativo, 
                                                                                  UsuarioInclusaoId, DataInclusao, 
                                                                                  NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@AcademiaId, @EnderecoId, 1, @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insertVinculo, new
                    {
                        AcademiaId = academiaId,
                        EnderecoId = novoId,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);
                }
            }

            // Atualiza os existentes
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

            // Remove os que não foram enviados
            var paraExcluir = atuais.Except(recebidos);
            foreach (var id in paraExcluir)
            {
                // Remove o vínculo
                await _connection.ExecuteAsync("DELETE FROM AcademiaEndereco WHERE AcademiaId = @AcademiaId AND EnderecoId = @EnderecoId",
                    new { AcademiaId = academiaId, EnderecoId = id }, transaction);

                // Remove o endereço (assumindo que é exclusivo da academia)
                await _connection.ExecuteAsync("DELETE FROM Endereco WHERE EnderecoId = @EnderecoId",
                    new { EnderecoId = id }, transaction);
            }
        }
        //📞 2. Sincronizar Telefones (mesma lógica)
        private async Task SincronizarTelefonesAsync(int academiaId, IEnumerable<Telefone> novosTelefones, int usuarioId, IDbTransaction transaction)
        {
            const string sqlAtuais = @"
                                        SELECT t.TelefoneId
                                        FROM Telefone t
                                        INNER JOIN AcademiaTelefone at ON at.TelefoneId = t.TelefoneId
                                        WHERE at.AcademiaId = @AcademiaId AND at.Ativo = 1";

            var atuais = (await _connection.QueryAsync<int>(sqlAtuais, new { AcademiaId = academiaId }, transaction)).ToHashSet();

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
                                                    SELECT CAST(SCOPE_IDENTITY() as int);";

                    var novoId = await _connection.QuerySingleAsync<int>(insertTelefone, new
                    {
                        tel.NumeroTelefone,
                        tel.TipoTelefoneId,
                        Ativo = true,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);

                    const string insertVinculo = @"
                                                    INSERT INTO AcademiaTelefone (AcademiaId, TelefoneId, Ativo, 
                                                                                  UsuarioInclusaoId, DataInclusao, 
                                                                                  NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@AcademiaId, @TelefoneId, 1, @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insertVinculo, new
                    {
                        AcademiaId = academiaId,
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
                await _connection.ExecuteAsync("DELETE FROM AcademiaTelefone WHERE AcademiaId = @AcademiaId AND TelefoneId = @TelefoneId",
                    new { AcademiaId = academiaId, TelefoneId = id }, transaction);

                await _connection.ExecuteAsync("DELETE FROM Telefone WHERE TelefoneId = @TelefoneId",
                    new { TelefoneId = id }, transaction);
            }
        }
        //🌐 3. Sincronizar Redes Sociais (sem deletar a tabela RedeSocial)
        private async Task SincronizarRedesSociaisAsync(int academiaId, IEnumerable<AcademiaRedeSocial> novasRedes, int usuarioId, IDbTransaction transaction)
        {
            const string sqlAtuais = @"
                                        SELECT AcademiaRedeSocialId, RedeSocialId
                                        FROM AcademiaRedeSocial
                                        WHERE AcademiaId = @AcademiaId AND Ativo = 1";

            var atuais = await _connection.QueryAsync<(int IdVinculo, int RedeSocialId)>(sqlAtuais, new { AcademiaId = academiaId }, transaction);

            var atuaisMap = atuais.ToDictionary(x => x.RedeSocialId, x => x.IdVinculo);
            var recebidos = new HashSet<int>();

            foreach (var rede in novasRedes)
            {
                if (rede.RedeSocialId <= 0) continue;

                if (atuaisMap.ContainsKey(rede.RedeSocialId))
                {
                    // Atualiza o perfil
                    const string update = @"
                                            UPDATE AcademiaRedeSocial SET
                                                PerfilUrl = @PerfilUrl,
                                                UsuarioOperacaoId = @UsuarioOperacaoId,
                                                DataOperacao = GETDATE()
                                            WHERE AcademiaRedeSocialId = @AcademiaRedeSocialId";

                    await _connection.ExecuteAsync(update, new
                    {
                        PerfilUrl = rede.PerfilUrl,
                        UsuarioOperacaoId = usuarioId,
                        AcademiaRedeSocialId = atuaisMap[rede.RedeSocialId]
                    }, transaction);

                    recebidos.Add(rede.RedeSocialId);
                }
                else
                {
                    // Nova rede
                    const string insert = @"
                                            INSERT INTO AcademiaRedeSocial 
                                                (AcademiaId, RedeSocialId, PerfilUrl, Ativo, 
                                                 UsuarioInclusaoId, DataInclusao, NaturezaOperacao, 
                                                 UsuarioOperacaoId, DataOperacao)
                                            VALUES 
                                                (@AcademiaId, @RedeSocialId, @PerfilUrl, 1, 
                                                 @UsuarioInclusaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insert, new
                    {
                        AcademiaId = academiaId,
                        rede.RedeSocialId,
                        rede.PerfilUrl,
                        UsuarioInclusaoId = usuarioId,
                        UsuarioOperacaoId = usuarioId
                    }, transaction);
                }
            }

            // Remove as que não foram enviadas
            var paraExcluir = atuais.Where(x => !recebidos.Contains(x.RedeSocialId));
            foreach (var item in paraExcluir)
            {
                await _connection.ExecuteAsync("DELETE FROM AcademiaRedeSocial WHERE AcademiaRedeSocialId = @AcademiaRedeSocialId", new { AcademiaRedeSocialId = item.IdVinculo }, transaction);
            }
        }
        public async Task<bool> InativarAcademiaPorIdAsync(int academiaId, int usuarioOperacaoId)
        {
            try
            {
                if (academiaId <= 0)
                    throw new ArgumentException("ID da academia inválido", nameof(academiaId));

                if (usuarioOperacaoId <= 0)
                    throw new ArgumentException("ID do usuário operador inválido", nameof(usuarioOperacaoId));

                const string sql = @"
                                    UPDATE Academia
                                    SET Ativo = 0,
                                        DataOperacao = GETDATE(),
                                        UsuarioOperacaoId = @usuarioOperacaoId,
                                        NaturezaOperacao = 'A'
                                    WHERE AcademiaId = @academiaId";

                var rowsAffected = await _connection.ExecuteAsync(sql, new
                {
                    academiaId,
                    usuarioOperacaoId
                });

                if (rowsAffected == 0)
                    throw new AcademiaNaoEncontradaException(academiaId);

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao inativar academia {AcademiaId} pelo usuário {UsuarioOperacaoId}", academiaId, usuarioOperacaoId);
                throw new ExclusaoAcademiaException(academiaId, ex);
            }
        }
    }
}