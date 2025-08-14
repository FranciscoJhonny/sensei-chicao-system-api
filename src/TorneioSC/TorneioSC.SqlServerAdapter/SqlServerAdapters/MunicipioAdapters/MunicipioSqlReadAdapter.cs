using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionMunicipio;
using TorneioSC.SqlServerAdapter.Context;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.MunicipioAdapters
{
    public class MunicipioSqlReadAdapter : IMunicipioSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<MunicipioSqlReadAdapter> _logger;

        static MunicipioSqlReadAdapter() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        public MunicipioSqlReadAdapter(SqlServerAdapterConfiguration configuration, ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<MunicipioSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        public async Task<IEnumerable<Municipio>> ObterMunicipioAsync()
        {
            try
            {
                const string sql = @"SELECT MunicipioId
                                      ,EstadoId
                                      ,DescricaoMunicio
                                      ,DataInclusao
                                      ,DataOperacao
                                      ,Ativo
                                   FROM Municipio";

                return await _connection.QueryAsync<Municipio>(sql, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter municipios completas");
                throw new OperacaoMunicipioException("consulta completa", ex);
            }

        }

        public async Task<Municipio?> ObterMunicipioPorIdAsync(int municipioId)
        {
            try
            {
                const string sql = @"SELECT MunicipioId
                                       ,EstadoId
                                      ,DescricaoMunicio
                                      ,DataInclusao
                                      ,DataOperacao
                                      ,Ativo
                                   FROM Municipio
                                WHERE MunicipioId = @MunicipioId";

                return await _connection.QueryFirstOrDefaultAsync<Municipio>(sql, new { municipioId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar munícipio por ID: {municipioId}");
                throw new OperacaoMunicipioException("busca por ID", ex);
            }
            
        }

        public async Task<IEnumerable<Municipio>> ObterMunicipioPorEstadoIdAsync(int estadoId)
        {
            try
            {
                const string sql = @"SELECT MunicipioId
                                      ,EstadoId
                                      ,DescricaoMunicio
                                      ,DataInclusao
                                      ,DataOperacao
                                      ,Ativo
                                   FROM Municipio
                                WHERE EstadoId = @estadoId";

                return await _connection.QueryAsync<Municipio>(sql, new { estadoId }, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar estado por ID: {estadoId}");
                throw new OperacaoMunicipioException("busca por ID", ex);
            }
           
        }
    }
}
