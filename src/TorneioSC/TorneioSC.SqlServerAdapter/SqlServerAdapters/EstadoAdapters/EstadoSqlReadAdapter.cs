using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionEstado;
using TorneioSC.Exception.ExceptionBase.ExceptionMunicipio;
using TorneioSC.SqlServerAdapter.Context;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.EstadoAdapters
{
    public class EstadoSqlReadAdapter : IEstadoSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<EstadoSqlReadAdapter> _logger;

        static EstadoSqlReadAdapter() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        public EstadoSqlReadAdapter(SqlServerAdapterConfiguration configuration, ILoggerFactory loggerFactory, SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<EstadoSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        public async Task<IEnumerable<Estado>> ObterEstadoAsync()
        {
            try
            {
                const string sql = @"SELECT EstadoId
                                       ,DescricaoEstado
                                       ,DataInclusao
                                      ,DataOperacao
                                       ,Ativo
                                       ,Sigla
                                   FROM Estado";

                return await _connection.QueryAsync<Estado>(sql, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter estados completas");
                throw new OperacaoMunicipioException("consulta completa", ex);
            }

        }

        public async Task<Estado?> ObterEstadoPorIdAsync(int estadoId)
        {
            try
            {
                const string sql = @"SELECT e.EstadoId,
                                    e.DescricaoEstado,
                                    e.DataInclusao,
                                    e.DataOperacao,
                                    e.Ativo,
                                    e.Sigla,
                                    m.MunicipioId,
                                    m.EstadoId,
                                    m.DescricaoMunicio,
                                    m.DataInclusao,
                                    m.DataOperacao,
                                    m.Ativo
                             FROM Estado e
                             LEFT JOIN Municipio m ON m.EstadoId = e.EstadoId
                             WHERE e.EstadoId = @estadoId";

                var lookup = new Dictionary<int, Estado>();

                await _connection.QueryAsync<Estado, Municipio, Estado>(
                    sql,
                    (estado, municipio) =>
                    {
                        if (!lookup.TryGetValue(estado.EstadoId, out var estadoEntry))
                        {
                            estadoEntry = estado;
                            estadoEntry.Municipios = new List<Municipio>();
                            lookup.Add(estadoEntry.EstadoId, estadoEntry);
                        }

                        if (municipio != null)
                        {
                            estadoEntry.Municipios.Add(municipio);
                        }

                        return estadoEntry;
                    },
                    new { estadoId },
                    splitOn: "MunicipioId");

                return lookup.Values.FirstOrDefault();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Erro ao buscar estado por ID: {estadoId}");
                throw new OperacaoEstadoException("busca por ID", ex);
            }
        }
    }
}
