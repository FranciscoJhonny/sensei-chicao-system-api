using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.FederacaoAdapters;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.AcademiaAdapters
{
    internal class AcademiaSqlReadAdapter : IAcademiaSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<FederacaoSqlReadAdapter> _logger;

        public AcademiaSqlReadAdapter(ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<FederacaoSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        public async Task<int> ObterTotalAcademiasAtivasAsync()
        {
            const string sql = "SELECT COUNT(*) FROM Academia WHERE Ativo = 1";

            try
            {
                return await _connection.ExecuteScalarAsync<int>(sql);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter total de academias ativas");
                throw new OperacaoAcademiaException("contagem de academias ativas", ex);
            }
        }
        public async Task<int> ObterTotalAcademiasAsync(FiltroAcademia? filtro = null)
        {
            var sql = new StringBuilder("SELECT COUNT(*) FROM Academia WHERE 1 = 1");
            var parameters = new DynamicParameters();

            if (filtro != null)
            {
                if (filtro.Ativo.HasValue)
                {
                    sql.Append(" AND Ativo = @Ativo");
                    parameters.Add("Ativo", filtro.Ativo.Value);
                }

                if (!string.IsNullOrEmpty(filtro.Nome))
                {
                    sql.Append(" AND Nome LIKE @Nome");
                    parameters.Add("Nome", $"%{filtro.Nome}%");
                }

                if (!string.IsNullOrEmpty(filtro.Cnpj))
                {

                    parameters.Add("Cnpj", filtro.Cnpj);
                }

                if (filtro.MunicipioId.HasValue)
                {
                    sql.Append(" AND MunicipioId = @MunicipioId");
                    parameters.Add("MunicipioId", filtro.MunicipioId.Value);
                }

                if (filtro.FederacaoId.HasValue)
                {
                    sql.Append(" AND FederacaoId = @FederacaoId");
                    parameters.Add("FederacaoId", filtro.FederacaoId.Value);
                }
            }

            try
            {
                return await _connection.ExecuteScalarAsync<int>(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter total de academias com filtros");
                throw new OperacaoAcademiaException("contagem de academias com filtros", ex);
            }
        }
        public async Task<Academia?> ObterPorCnpjAsync(string Cnpj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Cnpj))
                    throw new ArgumentException("CNPJ não pode ser vazio ou nulo", nameof(Cnpj));

               

                if (Cnpj.Length != 14)
                    throw new ArgumentException("CNPJ deve conter 14 dígitos", nameof(Cnpj));

                const string sql = @"SELECT * FROM Academia WHERE CNPJ = @cnpj";

                var academia = await _connection.QueryFirstOrDefaultAsync<Academia>(sql, new { Cnpj });

                return academia;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar academia por CNPJ: {CNPJ}", Cnpj);
                throw new OperacaoAcademiaException("Erro ao buscar academia por CNPJ", ex);
            }
        }
        public async Task<Academia?> ObterAcademiaPorIdAsync(int academiaId)
        {
            try
            {
                if (academiaId <= 0)
                    throw new ArgumentException("ID da academia inválido", nameof(academiaId));

                const string sql = @"SELECT 
                                         a.*,
                                         e.*,
                                         t.*,
                                         rs.*
                                     FROM Academia a
                                     LEFT JOIN AcademiaEndereco ae ON a.AcademiaId = ae.AcademiaId
                                     LEFT JOIN Endereco e ON ae.EnderecoId = e.EnderecoId
                                     LEFT JOIN AcademiaTelefone at ON a.AcademiaId = at.AcademiaId
                                     LEFT JOIN Telefone t ON at.TelefoneId = t.TelefoneId
                                     LEFT JOIN AcademiaRedeSocial ars ON a.AcademiaId = ars.AcademiaId
                                     LEFT JOIN RedeSocial rs ON ars.RedeSocialId = rs.RedeSocialId
                                     WHERE a.AcademiaId = @academiaId
                                     AND a.Ativo = 1";

                var lookup = new Dictionary<int, Academia>();
                await _connection.QueryAsync<Academia, Endereco, Telefone, AcademiaRedeSocial, Academia>(
                    sql,
                    (academia, endereco, telefone, redeSocial) =>
                    {
                        if (!lookup.TryGetValue(academia.AcademiaId, out var academiaEntry))
                        {
                            academiaEntry = academia;
                            academiaEntry.Enderecos = new List<Endereco>();
                            academiaEntry.Telefones = new List<Telefone>();
                            academiaEntry.AcademiaRedeSociais = new List<AcademiaRedeSocial>();
                            lookup.Add(academiaEntry.AcademiaId, academiaEntry);
                        }

                        if (endereco != null && !academiaEntry.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                            academiaEntry.Enderecos.Add(endereco);

                        if (telefone != null && !academiaEntry.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                            academiaEntry.Telefones.Add(telefone);

                        if (redeSocial != null && !academiaEntry.AcademiaRedeSociais.Any(r => r.RedeSocialId == redeSocial.RedeSocialId))
                            academiaEntry.AcademiaRedeSociais.Add(redeSocial);

                        return academiaEntry;
                    },
                    new { academiaId },
                    splitOn: "EnderecoId,TelefoneId,RedeSocialId");

                return lookup.Values.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao obter academia por ID: {academiaId}");
                throw new OperacaoAcademiaException("busca por ID", ex);
            }
        }
        public async Task<Academia?> ObterPorCnpjUpdateAsync(string Cnpj, int academiaId)
        {
            try
            {
                const string sql = "SELECT * FROM Academia WHERE CNPJ = @cnpj AND AcademiaId != @academiaId";
                return await _connection.QueryFirstOrDefaultAsync<Academia>(sql, new { Cnpj, academiaId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar academia por CNPJ: {Cnpj}");
                throw new OperacaoAcademiaException("busca por CNPJ", ex);
            }
        }
        public async Task<EstatisticasAcademias> ObterEstatisticasAcademiasAsync()
        {
            const string sql = @"
                                SELECT 
                                    COUNT(*) as Total,
                                    SUM(CASE WHEN Ativo = 1 THEN 1 ELSE 0 END) as Ativas,
                                    SUM(CASE WHEN Ativo = 0 THEN 1 ELSE 0 END) as Inativas,
                                    COUNT(DISTINCT MunicipioId) as CidadesDiferentes,
                                    COUNT(DISTINCT FederacaoId) as FederacoesDiferentes
                                FROM Academia";

            try
            {
                var resultado = await _connection.QueryFirstOrDefaultAsync<EstatisticasAcademias>(sql);

                // Retorna um objeto padrão se for nulo
                return resultado ?? new EstatisticasAcademias
                {
                    Total = 0,
                    Ativas = 0,
                    Inativas = 0,
                    CidadesDiferentes = 0,
                    FederacoesDiferentes = 0
                };
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter estatísticas das academias");
                throw new OperacaoAcademiaException("obtenção de estatísticas", ex);
            }
        }
        public async Task<IEnumerable<Academia>> ObterAcademiasAsync()
        {
            try
            {
                const string sql = @"SELECT * FROM Academia WHERE Ativo = 1 ORDER BY Nome";

                var academias = await _connection.QueryAsync<Academia>(sql);

                if (!academias.Any())
                {
                    _logger.LogWarning("Nenhuma academia ativa encontrada");
                }

                return academias;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro SQL ao obter lista de academias");
                throw new OperacaoAcademiaException("listagem", ex);
            }
        }
        public async Task<(IEnumerable<Academia> Academias, int Total)> ObterAcademiasPorFiltroAsync(FiltroAcademia filtro)
        {
            var sql = @"
                        SELECT 
                            a.AcademiaId,
                            a.Nome,
                            a.Email,
                            a.Cnpj,
                            a.Ativo,
                            a.DataInclusao,
                            a.DataOperacao,
                            f.FederacaoId as FederacaoId,
                            f.Nome as FederacaoNome,
                            m.MunicipioId as MunicipioId,
                            m.DescricaoMunicio as MunicipioNome,
                            e.DescricaoEstado as UF,
                            COUNT(*) OVER() as TotalCount
                        FROM Academia a
                        LEFT JOIN dbo.Federacao f ON a.FederacaoId = f.FederacaoId
                        LEFT JOIN dbo.Municipio m ON a.MunicipioId = m.MunicipioId
                        LEFT JOIN dbo.Estado e ON e.EstadoId = m.EstadoId
                        WHERE (@Nome IS NULL OR a.Nome LIKE '%' + @Nome + '%')
                          AND (@Cnpj IS NULL OR a.Cnpj LIKE '%' + @Cnpj + '%')
                          AND (@MunicipioId IS NULL OR a.MunicipioId = @MunicipioId)
                          AND (@FederacaoId IS NULL OR a.FederacaoId = @FederacaoId)
                          AND (@Ativo IS NULL OR a.Ativo = @Ativo)
                        ORDER BY a.Nome
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var parameters = new
            {
                filtro.Nome,
                filtro.Cnpj,
                filtro.MunicipioId,
                filtro.FederacaoId,
                filtro.Ativo,
                Offset = (filtro.Pagina - 1) * filtro.TamanhoPagina,
                PageSize = filtro.TamanhoPagina
            };

            try
            {
                var academias = await _connection.QueryAsync<Academia, Federacao, Municipio, Academia>(
                    sql,
                    (academia, federacao, municipio) =>
                    {
                        academia.Federacao = federacao;
                        academia.Municipio = municipio;
                        return academia;
                    },
                    parameters,
                    splitOn: "FederacaoId,MunicipioId"
                );

                // Para obter o total, precisamos de uma query separada
                var totalSql = @"
                                SELECT COUNT(*)
                                FROM Academia a
                                WHERE (@Nome IS NULL OR a.Nome LIKE '%' + @Nome + '%')
                                  AND (@Cnpj IS NULL OR a.Cnpj LIKE '%' + @Cnpj + '%')
                                  AND (@MunicipioId IS NULL OR a.MunicipioId = @MunicipioId)
                                  AND (@FederacaoId IS NULL OR a.FederacaoId = @FederacaoId)
                                  AND (@Ativo IS NULL OR a.Ativo = @Ativo)";

                var total = await _connection.ExecuteScalarAsync<int>(totalSql, parameters);

                return (academias, total);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar academias com filtros");
                throw;
            }
        }              
        public async Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync()
        {
            const string sql = @"SELECT  a.AcademiaId ,
                                         a.Nome ,
                                         a.CNPJ ,
                                         m.DescricaoMunicio AS Cidade ,
                                         a.Ativo
                                 FROM    Academia a
                                         LEFT JOIN AcademiaEndereco ae ON a.AcademiaId = ae.AcademiaId
                                                                          AND ae.Ativo = 1
                                         LEFT JOIN Endereco e ON ae.EnderecoId = e.EnderecoId
                                                                 AND e.Ativo = 1
                                         LEFT JOIN dbo.Municipio m ON m.MunicipioId = a.MunicipioId
                                 WHERE   a.Ativo = 1
                                 ORDER BY a.Nome;";

            try
            {
                var academias = await _connection.QueryAsync<AcademiaResumo>(sql);

                return academias ?? Enumerable.Empty<AcademiaResumo>();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das academias");
                throw new OperacaoAcademiaException("obtenção do resumo", ex);
            }
        }
        public async Task<(IEnumerable<AcademiaResumo> Resumos, int Total)> ObterResumoAcademiasPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            const string sql = @"SELECT  a.AcademiaId ,
                                         a.Nome ,
                                         a.CNPJ ,
                                         m.DescricaoMunicio AS Cidade ,
                                         a.Ativo
                                 FROM    Academia a
                                         LEFT JOIN AcademiaEndereco ae ON a.AcademiaId = ae.AcademiaId
                                                                          AND ae.Ativo = 1
                                         LEFT JOIN Endereco e ON ae.EnderecoId = e.EnderecoId
                                                                 AND e.Ativo = 1
                                         LEFT JOIN dbo.Municipio m ON m.MunicipioId = a.MunicipioId
                                 WHERE   a.Ativo = 1
                                 ORDER BY a.Nome
                                         OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                                         
                                 SELECT  COUNT(*)
                                 FROM    Academia
                                 WHERE   Ativo = 1;";

            try
            {
                using var multi = await _connection.QueryMultipleAsync(sql, new
                {
                    Offset = (pagina - 1) * tamanhoPagina,
                    PageSize = tamanhoPagina
                });

                var resumos = await multi.ReadAsync<AcademiaResumo>();
                var total = await multi.ReadSingleAsync<int>();

                return (resumos, total);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo paginado das academias");
                throw new OperacaoAcademiaException("obtenção do resumo paginado", ex);
            }
        }        
        public async Task<IEnumerable<AcademiaResumo>> ObterResumoAcademiasAsync(FiltroAcademia filtro)
        {
            var sql = new StringBuilder(@"SELECT  a.AcademiaId ,
                                                  a.Nome ,
                                                  a.CNPJ ,
                                                  m.DescricaoMunicio AS Cidade ,
                                                  a.Ativo
                                          FROM    Academia a
                                                  LEFT JOIN AcademiaEndereco ae ON a.AcademiaId = ae.AcademiaId
                                                                                   AND ae.Ativo = 1
                                                  LEFT JOIN Endereco e ON ae.EnderecoId = e.EnderecoId
                                                                          AND e.Ativo = 1
                                                  LEFT JOIN dbo.Municipio m ON m.MunicipioId = a.MunicipioId
                                          WHERE   a.Ativo = 1 ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                sql.Append(" AND a.Nome LIKE @Nome");
                parameters.Add("Nome", $"%{filtro.Nome}%");
            }

            if (!string.IsNullOrEmpty(filtro.Cnpj))
            {
                parameters.Add("Cnpj", filtro.Cnpj);
            }

            if (filtro.MunicipioId.HasValue)
            {
                sql.Append(" AND m.MunicipioId = @MunicipioId");
                parameters.Add("MunicipioId", filtro.MunicipioId.Value);
            }

            sql.Append(" ORDER BY a.Nome");

            try
            {
                return await _connection.QueryAsync<AcademiaResumo>(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo filtrado das academias");
                throw new OperacaoAcademiaException("obtenção do resumo filtrado", ex);
            }
        }        
    }
}
