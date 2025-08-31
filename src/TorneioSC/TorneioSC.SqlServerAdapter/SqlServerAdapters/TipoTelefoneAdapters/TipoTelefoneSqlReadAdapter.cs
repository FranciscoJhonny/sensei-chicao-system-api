using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionTipoTelefone;
using TorneioSC.SqlServerAdapter.Context;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.TipoTelefoneAdapters
{
    /// <summary>
    /// Adaptador de leitura para operações de consulta relacionadas à entidade TipoTelefone.
    /// Responsável por buscar tipos de telefone no banco de dados.
    /// </summary>
    public class TipoTelefoneSqlReadAdapter : ITipoTelefoneSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<TipoTelefoneSqlReadAdapter> _logger;

        /// <summary>
        /// Inicializa o mapeamento de tipo do Dapper para garantir que strings sejam tratadas como ANSI.
        /// </summary>
        static TipoTelefoneSqlReadAdapter() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TipoTelefoneSqlReadAdapter"/>.
        /// </summary>
        /// <param name="configuration">Configuração do adaptador SQL (opcional, pode ser útil para extensões futuras).</param>
        /// <param name="loggerFactory">Fábrica de loggers para criar instâncias de <see cref="ILogger"/>.</param>
        /// <param name="context">Contexto de conexão com o banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="context"/> ou <paramref name="loggerFactory"/> forem nulos.</exception>
        public TipoTelefoneSqlReadAdapter(
            SqlServerAdapterConfiguration configuration,
            ILoggerFactory loggerFactory,
            SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<TipoTelefoneSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        /// <summary>
        /// Obtém todos os tipos de telefone ativos do sistema.
        /// </summary>
        /// <returns>Lista de todos os tipos de telefone cadastrados.</returns>
        /// <exception cref="OperacaoTipoTelefoneException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<IEnumerable<TipoTelefone>> ObterTipoTelefoneAsync()
        {
            try
            {
                const string sql = @"SELECT 
                                        TipoTelefoneId,
                                        DescricaoTipoTelefone,
                                        DataInclusao,
                                        DataOperacao,
                                        Ativo
                                     FROM TipoTelefone
                                     WHERE Ativo = 1
                                     ORDER BY DescricaoTipoTelefone";

                return await _connection.QueryAsync<TipoTelefone>(sql, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter tipos de telefone");
                throw new OperacaoTipoTelefoneException("consulta completa", ex);
            }
        }

        /// <summary>
        /// Obtém um tipo de telefone específico pelo seu ID.
        /// </summary>
        /// <param name="tipoTelefoneId">ID do tipo de telefone a ser buscado.</param>
        /// <returns>O tipo de telefone encontrado, ou <c>null</c> se não existir.</returns>
        /// <exception cref="OperacaoTipoTelefoneException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<TipoTelefone?> ObterTipoTelefonePorIdAsync(int tipoTelefoneId)
        {
            try
            {
                const string sql = @"SELECT 
                                        TipoTelefoneId,
                                        DescricaoTipoTelefone,
                                        DataInclusao,
                                        DataOperacao,
                                        Ativo
                                     FROM TipoTelefone
                                     WHERE TipoTelefoneId = @tipoTelefoneId AND Ativo = 1";

                var tipoTelefone = await _connection.QueryFirstOrDefaultAsync<TipoTelefone>(sql, new { tipoTelefoneId });

                if (tipoTelefone == null)
                {
                    _logger.LogWarning("TipoTelefone com ID {TipoTelefoneId} não encontrado", tipoTelefoneId);
                }

                return tipoTelefone;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar tipo de telefone por ID: {TipoTelefoneId}", tipoTelefoneId);
                throw new OperacaoTipoTelefoneException("busca por ID", ex);
            }
        }
    }
}