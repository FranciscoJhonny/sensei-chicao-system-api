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

        public FederacaoSqlReadAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<FederacaoSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

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
                                            f.FederacaoId,
                                            f.Nome,
                                            f.MunicipioId,   
                                            f.CNPJ,
                                            f.Email,
                                            f.Site,
                                            f.DataFundacao,
                                            f.Portaria,
                                            f.Ativo,
                                            f.UsuarioInclusaoId,
                                            f.DataInclusao,
                                            f.NaturezaOperacao,
                                            f.UsuarioOperacaoId,
                                            f.DataOperacao,
                                            m.MunicipioId,
                                            m.DescricaoMunicio,
                                            m.EstadoId,
                                            es.EstadoId,
                                            es.DescricaoEstado,
                                            es.Sigla,
                                            e.EnderecoId,
                                            e.Logradouro,
                                            e.Numero,
                                            e.Complemento,
                                            e.Cep,
                                            e.Bairro,
                                            t.TelefoneId,
                                            t.NumeroTelefone,
                                            t.TipoTelefoneId,
                                            tt.TipoTelefoneId,
                                            tt.DescricaoTipoTelefone,
                                            usIncl.Nome,
                                            usOp.Nome
                                        FROM dbo.Federacao f
                                        JOIN dbo.Municipio m ON m.MunicipioId = f.MunicipioId
                                        JOIN dbo.Estado es ON es.EstadoId = m.EstadoId
                                        LEFT JOIN dbo.FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                                        LEFT JOIN dbo.Endereco e ON e.EnderecoId = fe.EnderecoId
                                        LEFT JOIN dbo.FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                                        LEFT JOIN dbo.Telefone t ON t.TelefoneId = ft.TelefoneId
                                        LEFT JOIN dbo.TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                                        LEFT JOIN dbo.Usuario usIncl ON usIncl.UsuarioId = f.UsuarioInclusaoId
                                        LEFT JOIN dbo.Usuario usOp ON usOp.UsuarioId = f.UsuarioOperacaoId
                            WHERE f.Ativo = 1";

                var federacoes = new Dictionary<int, Federacao>();

                await _connection.QueryAsync<Federacao, Municipio, Estado, Endereco, Telefone, TipoTelefone, Federacao>(
                    sql,
                    (f, municipio, estado, endereco, telefone, tipoTelefone) =>
                    {
                        if (!federacoes.TryGetValue(f.FederacaoId, out var federacaoEntry))
                        {
                            federacaoEntry = f;
                            federacaoEntry.Enderecos = new List<Endereco>();
                            federacaoEntry.Telefones = new List<Telefone>();
                            federacaoEntry.Municipio = municipio;
                            federacaoEntry.Municipio.Estado = estado;
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
                    splitOn: "MunicipioId,EstadoId,EnderecoId,TelefoneId,TipoTelefoneId");

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
                const string sql = @"SELECT 
                                            f.FederacaoId,
                                            f.Nome,
                                            f.MunicipioId,   
                                            f.CNPJ,
                                            f.Email,
                                            f.Site,
                                            f.DataFundacao,
                                            f.Portaria,
                                            f.Ativo,
                                            f.UsuarioInclusaoId,
                                            f.DataInclusao,
                                            f.NaturezaOperacao,
                                            f.UsuarioOperacaoId,
                                            f.DataOperacao,
                                            m.MunicipioId,
                                            m.DescricaoMunicio,
                                            m.EstadoId,
                                            es.EstadoId,
                                            es.DescricaoEstado,
                                            es.Sigla,
                                            e.EnderecoId,
                                            e.Logradouro,
                                            e.Numero,
                                            e.Complemento,
                                            e.Cep,
                                            e.Bairro,
                                            t.TelefoneId,
                                            t.NumeroTelefone,
                                            t.TipoTelefoneId,
                                            tt.TipoTelefoneId,
                                            tt.DescricaoTipoTelefone,
                                            usIncl.Nome,
                                            usOp.Nome
                                        FROM dbo.Federacao f
                                        JOIN dbo.Municipio m ON m.MunicipioId = f.MunicipioId
                                        JOIN dbo.Estado es ON es.EstadoId = m.EstadoId
                                        LEFT JOIN dbo.FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                                        LEFT JOIN dbo.Endereco e ON e.EnderecoId = fe.EnderecoId
                                        LEFT JOIN dbo.FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                                        LEFT JOIN dbo.Telefone t ON t.TelefoneId = ft.TelefoneId
                                        LEFT JOIN dbo.TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                                        LEFT JOIN dbo.Usuario usIncl ON usIncl.UsuarioId = f.UsuarioInclusaoId
                                        LEFT JOIN dbo.Usuario usOp ON usOp.UsuarioId = f.UsuarioOperacaoId
                                        WHERE f.FederacaoId = @federacaoId;";

                Federacao? federacao = null;

                await _connection.QueryAsync<Federacao, Municipio, Estado, Endereco, Telefone, TipoTelefone, Federacao>(
                    sql,
                    (f, municipio, estado, endereco, telefone, tipoTelefone) =>
                    {
                        if (federacao == null)
                        {
                            federacao = f;
                            federacao.Enderecos = new List<Endereco>();
                            federacao.Telefones = new List<Telefone>();
                            federacao.Municipio = municipio;
                            federacao.Municipio.Estado = estado;
                        }

                        if (endereco != null && endereco.EnderecoId != 0 &&
                            !federacao.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                        {
                            federacao.Enderecos.Add(endereco);
                        }

                        if (telefone != null && telefone.TelefoneId != 0)
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
                    splitOn: "MunicipioId,EstadoId,EnderecoId,TelefoneId,TipoTelefoneId");

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
                if (!string.IsNullOrWhiteSpace(cnpj))
                {
                    const string sql = "SELECT * FROM Federacao WHERE CNPJ = @cnpj";
                    var federacao = await _connection.QueryFirstOrDefaultAsync<Federacao>(sql, new { cnpj });

                    if (federacao != null)
                        throw new CnpjEmUsoException(cnpj);
                    return federacao;
                }

                return null;
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




        public async Task<int> PutFederacaoAsync(Federacao federacao)
        {
            if (federacao == null)
                throw new ArgumentNullException(nameof(federacao));

            if (federacao.FederacaoId <= 0)
                throw new ArgumentException("ID da federação inválido", nameof(federacao.FederacaoId));

            using var transaction = _connection.BeginTransaction();

            try
            {
                // Atualiza dados da Federação
                const string sqlFederacao = @"UPDATE Federacao
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

                // Obtém endereços atuais da federação
                const string sqlEnderecosAtuais = @"SELECT e.EnderecoId
                                                        FROM Endereco e
                                                        INNER JOIN FederacaoEndereco fe ON fe.EnderecoId = e.EnderecoId
                                                        WHERE fe.FederacaoId = @FederacaoId";

                var enderecosAtuaisIds = (await _connection.QueryAsync<int>(sqlEnderecosAtuais, new { federacao.FederacaoId }, transaction)).ToList();

                // ENDEREÇOS A EXCLUIR                
                foreach (var id in enderecosAtuaisIds)
                {
                    // Remove vínculo e endereço
                    const string deleteVinculo = "DELETE FROM FederacaoEndereco WHERE FederacaoId = @FederacaoId AND EnderecoId = @EnderecoId";
                    const string deleteEndereco = "DELETE FROM Endereco WHERE EnderecoId = @EnderecoId";

                    await _connection.ExecuteAsync(deleteVinculo, new { federacao.FederacaoId, EnderecoId = id }, transaction);
                    await _connection.ExecuteAsync(deleteEndereco, new { EnderecoId = id }, transaction);
                }
               
                
                foreach (var endereco in federacao.Enderecos)
                {
                    const string insertEndereco = @"
                                                    INSERT INTO Endereco (Logradouro, Numero, Complemento, Cep, Bairro, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@Logradouro, @Numero, @Complemento, @Cep, @Bairro, 1, @UsuarioOperacaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());
                                                    SELECT CAST(SCOPE_IDENTITY() as int);";

                    var novoEnderecoId = await _connection.QuerySingleAsync<int>(insertEndereco, endereco, transaction);

                    const string insertVinculo = @"
                                                    INSERT INTO FederacaoEndereco (FederacaoId, EnderecoId, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@FederacaoId, @EnderecoId, 1, @UsuarioOperacaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE())";

                    await _connection.ExecuteAsync(insertVinculo, new { federacao.FederacaoId, EnderecoId = novoEnderecoId, endereco.UsuarioOperacaoId}, transaction);
                }

                //Obtém telefones atuais da federação
                const string sqlTelefoneAtuais = @" SELECT  t.TelefoneId
                                                    FROM    Telefone t
                                                            INNER JOIN FederacaoTelefone ft ON ft.TelefoneId = t.TelefoneId
                                                    WHERE   ft.FederacaoId = @FederacaoId";

                var telefoneAtuaisIds = (await _connection.QueryAsync<int>(sqlTelefoneAtuais, new { federacao.FederacaoId }, transaction)).ToList();

                // TELEFONES A EXCLUIR                
                foreach (var id in telefoneAtuaisIds)
                {
                    // Remove vínculo e telefone
                    const string deleteVinculo = "DELETE FROM FederacaoTelefone WHERE FederacaoId = @FederacaoId AND TelefoneId = @TelefoneId";
                    const string deleteTelefone = "DELETE FROM Telefone WHERE TelefoneId = @TelefoneId";

                    await _connection.ExecuteAsync(deleteVinculo, new { federacao.FederacaoId, TelefoneId = id }, transaction);
                    await _connection.ExecuteAsync(deleteTelefone, new { TelefoneId = id }, transaction);
                }                

                // Insere os novos telefones e cria vinculação
                foreach (var telefone in federacao.Telefones)
                {
                    const string insertTelefone = @"INSERT INTO Telefone (NumeroTelefone, TipoTelefoneId, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                        VALUES (@NumeroTelefone, @TipoTelefoneId, 1, @UsuarioOperacaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());
                                                        SELECT CAST(SCOPE_IDENTITY() as int);";

                    int novoTelefoneId = await _connection.QuerySingleAsync<int>(insertTelefone, telefone, transaction);

                    const string insertVinculo = @"INSERT INTO FederacaoTelefone (FederacaoId, TelefoneId, Ativo, UsuarioInclusaoId, DataInclusao, NaturezaOperacao, UsuarioOperacaoId, DataOperacao)
                                                    VALUES (@FederacaoId, @TelefoneId, 1, @UsuarioOperacaoId, GETDATE(), 'I', @UsuarioOperacaoId, GETDATE());";

                    await _connection.ExecuteAsync(insertVinculo, new { federacao.FederacaoId, TelefoneId = novoTelefoneId, telefone.UsuarioOperacaoId }, transaction);
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