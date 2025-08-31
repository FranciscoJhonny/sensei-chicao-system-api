using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionEstado;
using TorneioSC.Exception.ExceptionBase.ExceptionMunicipio;
using TorneioSC.SqlServerAdapter.Context;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.EstadoAdapters
{
    /// <summary>
    /// Adaptador de leitura para operações de consulta relacionadas à entidade Estado.
    /// Responsável por buscar estados e seus municípios associados no banco de dados.
    /// </summary>
    public class EstadoSqlReadAdapter : IEstadoSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<EstadoSqlReadAdapter> _logger;

        /// <summary>
        /// Inicializa o mapeamento de tipo do Dapper para garantir que strings sejam tratadas como ANSI.
        /// </summary>
        static EstadoSqlReadAdapter() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        /// <summary>
        /// Inicializa uma nova instância do <see cref="EstadoSqlReadAdapter"/>.
        /// </summary>
        /// <param name="configuration">Configuração do adaptador SQL (não utilizada diretamente aqui, mas pode ser útil para extensões futuras).</param>
        /// <param name="loggerFactory">Fábrica de loggers para criar instâncias de <see cref="ILogger"/>.</param>
        /// <param name="context">Contexto de conexão com o banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="context"/> ou <paramref name="loggerFactory"/> forem nulos.</exception>
        public EstadoSqlReadAdapter(SqlServerAdapterConfiguration configuration,ILoggerFactory loggerFactory,SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<EstadoSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        /// <summary>
        /// Obtém todos os estados ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os estados.</returns>
        /// <exception cref="OperacaoMunicipioException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<IEnumerable<Estado>> ObterEstadoAsync()
        {
            try
            {
                const string sql = @"SELECT 
                                        EstadoId,
                                        DescricaoEstado,
                                        DataInclusao,
                                        DataOperacao,
                                        Ativo,
                                        Sigla
                                     FROM Estado";

                return await _connection.QueryAsync<Estado>(sql, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter estados completas");
                throw new OperacaoMunicipioException("consulta completa", ex);
            }
        }

        /// <summary>
        /// Obtém um estado específico pelo seu ID, incluindo todos os municípios associados.
        /// </summary>
        /// <param name="estadoId">ID do estado a ser buscado.</param>
        /// <returns>O estado encontrado com seus municípios, ou null se não encontrado.</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for inválido (não verificado diretamente aqui, mas pode ser adicionado).</exception>
        /// <exception cref="OperacaoEstadoException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<Estado?> ObterEstadoPorIdAsync(int estadoId)
        {
            try
            {
                const string sql = @"SELECT 
                                        e.EstadoId,
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
                _logger.LogError(ex, "Erro ao buscar estado por ID: {EstadoId}", estadoId);
                throw new OperacaoEstadoException("busca por ID", ex);
            }
        }
    }
}