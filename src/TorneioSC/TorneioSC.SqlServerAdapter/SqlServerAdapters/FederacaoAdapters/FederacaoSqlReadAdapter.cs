using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Dtos;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;
using System.Dynamic;

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

        public async Task<int> ObterTotalFederacoesAsync(FiltroFederacao? filtro = null)
        {
            var sql = new StringBuilder("SELECT COUNT(*) FROM Federacao WHERE Ativo = 1");
            var parameters = new DynamicParameters();

            if (filtro != null)
            {
                if (!string.IsNullOrEmpty(filtro.Nome))
                {
                    sql.Append(" AND Nome LIKE @Nome");
                    parameters.Add("Nome", $"%{filtro.Nome}%");
                }

                if (!string.IsNullOrEmpty(filtro.Cnpj))
                {
                    sql.Append(" AND CNPJ LIKE @Cnpj");
                    parameters.Add("Cnpj", $"%{filtro.Cnpj}%");
                }

                if (filtro.MunicipioId.HasValue)
                {
                    sql.Append(" AND MunicipioId = @MunicipioId");
                    parameters.Add("MunicipioId", filtro.MunicipioId.Value);
                }

                if (filtro.EstadoId.HasValue)
                {
                    sql.Append(" AND MunicipioId IN (SELECT MunicipioId FROM Municipio WHERE EstadoId = @EstadoId)");
                    parameters.Add("EstadoId", filtro.EstadoId.Value);
                }
            }

            try
            {
                return await _connection.ExecuteScalarAsync<int>(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao contar federações com filtros");
                throw new OperacaoFederacaoException("contagem com filtros", ex);
            }
        }
        public async Task<Federacao?> ObterPorCnpjAsync(string cnpj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                    return null;

                var cnpjNumeros = new string(cnpj.Where(char.IsDigit).ToArray());
                if (cnpjNumeros.Length != 14)
                    return null;

                const string sql = "SELECT * FROM Federacao WHERE CNPJ = @cnpj AND Ativo = 1";
                return await _connection.QueryFirstOrDefaultAsync<Federacao>(sql, new { cnpj = cnpjNumeros });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar federação por CNPJ: {CNPJ}", cnpj);
                throw new OperacaoFederacaoException("busca por CNPJ", ex);
            }
        }
        public async Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId)
        {
            try
            {
                if (federacaoId <= 0)
                    throw new ArgumentException("ID da federação inválido", nameof(federacaoId));

                const string sql = @"SELECT  
                                         f.*,
                                         e.*,
                                         t.*,
                                         frs.*,
                                         tt.*,
                                         rs.*,
                                         m.*,
                                         es.*
                                     FROM Federacao f
                                     LEFT JOIN FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                                     LEFT JOIN Endereco e ON e.EnderecoId = fe.EnderecoId AND e.Ativo = 1
                                     LEFT JOIN FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                                     LEFT JOIN Telefone t ON t.TelefoneId = ft.TelefoneId AND t.Ativo = 1
                                     LEFT JOIN TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                                     LEFT JOIN FederacaoRedeSocial frs ON frs.FederacaoId = f.FederacaoId
                                     LEFT JOIN RedeSocial rs ON rs.RedeSocialId = frs.RedeSocialId
                                     INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                                     INNER JOIN Estado es ON es.EstadoId = m.EstadoId
                                     WHERE f.FederacaoId = @federacaoId AND f.Ativo = 1";

                var lookup = new Dictionary<int, Federacao>();

                await _connection.QueryAsync<Federacao, Endereco, Telefone, FederacaoRedeSocial, TipoTelefone, Municipio, Estado, Federacao>(
                    sql,
                    (federacao, endereco, telefone, federacaoRedeSocial, tipoTelefone, municipio, estado) =>
                    {
                        if (!lookup.TryGetValue(federacao.FederacaoId, out var entry))
                        {
                            entry = federacao;
                            entry.Enderecos = new List<Endereco>();
                            entry.Telefones = new List<Telefone>();
                            entry.FederacaoRedeSociais = new List<FederacaoRedeSocial>();
                            entry.Municipio = municipio;
                            entry.Municipio.Estado = estado;
                            lookup.Add(entry.FederacaoId, entry);
                        }

                        if (endereco != null && !entry.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                            entry.Enderecos.Add(endereco);

                        if (telefone != null && tipoTelefone != null)
                        {
                            telefone.TipoTelefone = tipoTelefone;
                            if (!entry.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                                entry.Telefones.Add(telefone);
                        }

                        if (federacaoRedeSocial != null && !entry.FederacaoRedeSociais.Any(r => r.FederacaoRedeSocialId == federacaoRedeSocial.FederacaoRedeSocialId))
                            entry.FederacaoRedeSociais.Add(federacaoRedeSocial);

                        return entry;
                    },
                    new { federacaoId },
                    splitOn: "EnderecoId,TelefoneId,FederacaoRedeSocialId,TipoTelefoneId,MunicipioId,EstadoId");

                return lookup.Values.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar federação por ID: {FederacaoId}", federacaoId);
                throw new OperacaoFederacaoException("busca por ID", ex);
            }
        }
        public async Task<Federacao?> ObterPorCnpjUpdateAsync(string cnpj, int federacaoId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cnpj))
                    return null;

                var cnpjNumeros = new string(cnpj.Where(char.IsDigit).ToArray());
                if (cnpjNumeros.Length != 14)
                    return null;

                const string sql = "SELECT * FROM Federacao WHERE CNPJ = @cnpj AND FederacaoId != @federacaoId AND Ativo = 1";
                return await _connection.QueryFirstOrDefaultAsync<Federacao>(sql, new { cnpj = cnpjNumeros, federacaoId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao verificar CNPJ duplicado para federação: {CNPJ}, ID: {FederacaoId}", cnpj, federacaoId);
                throw new OperacaoFederacaoException("verificação de CNPJ", ex);
            }
        }
        public async Task<EstatisticasFederacoes> ObterEstatisticasFederacoesAsync()
        {
            const string sql = @"SELECT    COUNT(*) AS Total ,
                                            SUM(CASE WHEN f.Ativo = 1 THEN 1
                                                     ELSE 0
                                                END) AS Ativas ,
                                            SUM(CASE WHEN f.Ativo = 0 THEN 1
                                                     ELSE 0
                                                END) AS Inativas ,
                                            COUNT(DISTINCT EstadoId) AS EstadosDiferentes ,
                                            COUNT(DISTINCT f.MunicipioId) AS CidadesDiferentes
                                  FROM      Federacao f
                                            INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId;";

            try
            {
                var resultado = await _connection.QueryFirstOrDefaultAsync<EstatisticasFederacoes>(sql);
                return resultado ?? new EstatisticasFederacoes();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter estatísticas das federações");
                throw new OperacaoFederacaoException("obtenção de estatísticas", ex);
            }
        }
        public async Task<IEnumerable<Federacao>> ObterFederacaoAsync()
        {
            try
            {
                const string sql = @"
                    SELECT  
                        f.*,
                        e.*,
                        t.*,
                        tt.*,
                        m.*,
                        es.*
                    FROM Federacao f
                    LEFT JOIN FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                    LEFT JOIN Endereco e ON e.EnderecoId = fe.EnderecoId AND e.Ativo = 1
                    LEFT JOIN FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                    LEFT JOIN Telefone t ON t.TelefoneId = ft.TelefoneId AND t.Ativo = 1
                    LEFT JOIN TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                    INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                    INNER JOIN Estado es ON es.EstadoId = m.EstadoId
                    WHERE f.Ativo = 1";

                var federacoes = new Dictionary<int, Federacao>();

                await _connection.QueryAsync<Federacao, Endereco, Telefone, TipoTelefone, Municipio, Estado, Federacao>(
                    sql,
                    (f, endereco, telefone, tipoTelefone, municipio, estado) =>
                    {
                        if (!federacoes.TryGetValue(f.FederacaoId, out var entry))
                        {
                            entry = f;
                            entry.Enderecos = new List<Endereco>();
                            entry.Telefones = new List<Telefone>();
                            entry.Municipio = municipio;
                            entry.Municipio.Estado = estado;
                            federacoes.Add(entry.FederacaoId, entry);
                        }

                        if (endereco != null && !entry.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                            entry.Enderecos.Add(endereco);

                        if (telefone != null && tipoTelefone != null)
                        {
                            telefone.TipoTelefone = tipoTelefone;
                            if (!entry.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                                entry.Telefones.Add(telefone);
                        }

                        return entry;
                    },
                    splitOn: "EnderecoId,TelefoneId,TipoTelefoneId,MunicipioId,EstadoId");

                return federacoes.Values;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter federações");
                throw new OperacaoFederacaoException("listagem", ex);
            }
        }
        public async Task<(IEnumerable<Federacao> Federacoes, int Total)> ObterFederacoesPorFiltroAsync(FiltroFederacao filtro)
        {
            var sql = new StringBuilder(@"SELECT 
                                              f.*,
                                              e.*,
                                              t.*,
                                              frs.*,
                                              tt.*,
                                              m.*,
                                              es.*,
            
                                              rs.*
                                          FROM Federacao f
                                          LEFT JOIN FederacaoEndereco fe ON fe.FederacaoId = f.FederacaoId
                                          LEFT JOIN Endereco e ON e.EnderecoId = fe.EnderecoId AND e.Ativo = 1
                                          LEFT JOIN FederacaoTelefone ft ON ft.FederacaoId = f.FederacaoId
                                          LEFT JOIN Telefone t ON t.TelefoneId = ft.TelefoneId AND t.Ativo = 1
                                          LEFT JOIN TipoTelefone tt ON tt.TipoTelefoneId = t.TipoTelefoneId
                                          LEFT JOIN FederacaoRedeSocial frs ON frs.FederacaoId = f.FederacaoId
                                          LEFT JOIN RedeSocial rs ON rs.RedeSocialId = frs.RedeSocialId
                                          INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                                          INNER JOIN Estado es ON es.EstadoId = m.EstadoId
                                          WHERE f.Ativo = 1 ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                sql.Append(" AND f.Nome LIKE @Nome");
                parameters.Add("Nome", $"%{filtro.Nome}%");
            }

            if (!string.IsNullOrEmpty(filtro.Cnpj))
            {
                sql.Append(" AND f.CNPJ LIKE @Cnpj");
                parameters.Add("Cnpj", $"%{filtro.Cnpj}%");
            }

            if (filtro.MunicipioId.HasValue)
            {
                sql.Append(" AND f.MunicipioId = @MunicipioId");
                parameters.Add("MunicipioId", filtro.MunicipioId.Value);
            }

            if (filtro.EstadoId.HasValue)
            {
                sql.Append(" AND m.EstadoId = @EstadoId");
                parameters.Add("EstadoId", filtro.EstadoId.Value);
            }

            if (filtro.Ativo.HasValue)
            {
                sql.Append(" AND f.Ativo = @Ativo");
                parameters.Add("Ativo", filtro.Ativo.Value);
            }

            sql.Append(" ORDER BY f.Nome OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            parameters.Add("Offset", (filtro.Pagina - 1) * filtro.TamanhoPagina);
            parameters.Add("PageSize", filtro.TamanhoPagina);

            try
            {
                var lookup = new Dictionary<int, Federacao>();

                await _connection.QueryAsync<Federacao, Endereco, Telefone, FederacaoRedeSocial, TipoTelefone, Municipio, Estado, Federacao>(
                    sql.ToString(),
                    (federacao, endereco, telefone, federacaoRedeSocial, tipoTelefone, municipio, estado) =>
                    {
                        if (!lookup.TryGetValue(federacao.FederacaoId, out var entry))
                        {
                            entry = federacao;
                            entry.Enderecos = new List<Endereco>();
                            entry.Telefones = new List<Telefone>();
                            entry.FederacaoRedeSociais = new List<FederacaoRedeSocial>();
                            entry.Municipio = municipio;
                            entry.Municipio.Estado = estado;
                            lookup.Add(entry.FederacaoId, entry);
                        }

                        if (endereco != null && !entry.Enderecos.Any(e => e.EnderecoId == endereco.EnderecoId))
                            entry.Enderecos.Add(endereco);

                        if (telefone != null && tipoTelefone != null)
                        {
                            telefone.TipoTelefone = tipoTelefone;
                            if (!entry.Telefones.Any(t => t.TelefoneId == telefone.TelefoneId))
                                entry.Telefones.Add(telefone);
                        }

                        if (federacaoRedeSocial != null && !entry.FederacaoRedeSociais.Any(r => r.FederacaoRedeSocialId == federacaoRedeSocial.FederacaoRedeSocialId))
                            entry.FederacaoRedeSociais.Add(federacaoRedeSocial);

                        return entry;
                    },
                    parameters,
                    splitOn: "EnderecoId,TelefoneId,FederacaoRedeSocialId,TipoTelefoneId,MunicipioId,EstadoId");

                // Obter total
                var totalSql = $@"SELECT COUNT(*) FROM Federacao f
                                  INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                                  WHERE f.Ativo = 1";

                if (!string.IsNullOrEmpty(filtro.Nome))
                    totalSql += " AND f.Nome LIKE @Nome";
                if (!string.IsNullOrEmpty(filtro.Cnpj))
                    totalSql += " AND f.CNPJ LIKE @Cnpj";
                if (filtro.MunicipioId.HasValue)
                    totalSql += " AND f.MunicipioId = @MunicipioId";
                if (filtro.EstadoId.HasValue)
                    totalSql += " AND m.EstadoId = @EstadoId";
                if (filtro.Ativo.HasValue)
                    totalSql += " AND f.Ativo = @Ativo";

                var total = await _connection.ExecuteScalarAsync<int>(totalSql, parameters);

                return (lookup.Values, total);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar federações com filtros: {@Filtro}", filtro);
                throw new OperacaoFederacaoException("busca com filtros", ex);
            }
        }
        public async Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync()
        {
            const string sql = @"SELECT 
                                   f.FederacaoId,
                                   f.Nome,
                                   f.CNPJ,
                                   m.DescricaoMunicio AS Cidade,
                                   f.Ativo
                               FROM Federacao f
                               INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                               WHERE f.Ativo = 1
                               ORDER BY f.Nome";

            try
            {
                return await _connection.QueryAsync<FederacaoResumo>(sql);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo das federações");
                throw new OperacaoFederacaoException("obtenção do resumo", ex);
            }
        }
        public async Task<IEnumerable<FederacaoResumo>> ObterResumoFederacoesAsync(FiltroFederacao filtro)
        {
            var sql = new StringBuilder(@"SELECT 
                                            f.FederacaoId,
                                            f.Nome,
                                            f.CNPJ,
                                            m.DescricaoMunicio AS Cidade,
                                            f.Ativo
                                        FROM Federacao f
                                        INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                                        WHERE f.Ativo = 1 ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                sql.Append(" AND f.Nome LIKE @Nome");
                parameters.Add("Nome", $"%{filtro.Nome}%");
            }

            if (!string.IsNullOrEmpty(filtro.Cnpj))
            {
                sql.Append(" AND f.CNPJ LIKE @Cnpj");
                parameters.Add("Cnpj", $"%{filtro.Cnpj}%");
            }

            if (filtro.MunicipioId.HasValue)
            {
                sql.Append(" AND f.MunicipioId = @MunicipioId");
                parameters.Add("MunicipioId", filtro.MunicipioId.Value);
            }

            if (filtro.EstadoId.HasValue)
            {
                sql.Append(" AND m.EstadoId = @EstadoId");
                parameters.Add("EstadoId", filtro.EstadoId.Value);
            }

            sql.Append(" ORDER BY f.Nome");

            try
            {
                return await _connection.QueryAsync<FederacaoResumo>(sql.ToString(), parameters);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo filtrado das federações");
                throw new OperacaoFederacaoException("obtenção do resumo filtrado", ex);
            }
        }
        public async Task<(IEnumerable<FederacaoResumo> Resumos, int Total)> ObterResumoFederacoesPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            const string sql = @"SELECT 
                                   f.FederacaoId,
                                   f.Nome,
                                   f.CNPJ,
                                   m.DescricaoMunicio AS Cidade,
                                   f.Ativo
                               FROM Federacao f
                               INNER JOIN Municipio m ON m.MunicipioId = f.MunicipioId
                               WHERE f.Ativo = 1
                               ORDER BY f.Nome
                               OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                               
                               SELECT COUNT(*) FROM Federacao WHERE Ativo = 1";

            try
            {
                using var multi = await _connection.QueryMultipleAsync(sql, new
                {
                    Offset = (pagina - 1) * tamanhoPagina,
                    PageSize = tamanhoPagina
                });

                var resumos = await multi.ReadAsync<FederacaoResumo>();
                var total = await multi.ReadSingleAsync<int>();

                return (resumos, total);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo paginado das federações");
                throw new OperacaoFederacaoException("obtenção do resumo paginado", ex);
            }
        }
        
    }
}