using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Exception.ExceptionBase.ExceptionRedeSocial;
using TorneioSC.SqlServerAdapter.Context;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.RedeSocialAdapters
{
    /// <summary>
    /// Adaptador de leitura para operações de consulta relacionadas à entidade RedeSocial.
    /// Responsável por buscar redes sociais no banco de dados.
    /// </summary>
    public class RedeSocialSqlReadAdapter : IRedeSocialSqlReadAdapter
    {
        private readonly SqlConnection _connection;
        private readonly ILogger<RedeSocialSqlReadAdapter> _logger;

        /// <summary>
        /// Inicializa o mapeamento de tipo do Dapper para garantir que strings sejam tratadas como ANSI.
        /// </summary>
        static RedeSocialSqlReadAdapter() => SqlMapper.AddTypeMap(typeof(string), DbType.AnsiString);

        /// <summary>
        /// Inicializa uma nova instância do <see cref="RedeSocialSqlReadAdapter"/>.
        /// </summary>
        /// <param name="configuration">Configuração do adaptador SQL (opcional, pode ser útil para extensões futuras).</param>
        /// <param name="loggerFactory">Fábrica de loggers para criar instâncias de <see cref="ILogger"/>.</param>
        /// <param name="context">Contexto de conexão com o banco de dados.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="context"/> ou <paramref name="loggerFactory"/> forem nulos.</exception>
        public RedeSocialSqlReadAdapter(
            SqlServerAdapterConfiguration configuration,
            ILoggerFactory loggerFactory,
            SqlAdapterContext context)
        {
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<RedeSocialSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
                _connection.Open();
            }
        }

        /// <summary>
        /// Obtém todas as redes sociais ativas do sistema.
        /// </summary>
        /// <returns>Lista de todas as redes sociais cadastradas.</returns>
        /// <exception cref="OperacaoRedeSocialException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<IEnumerable<RedeSocial>> ObterRedeSocialAsync()
        {
            try
            {
                const string sql = @"SELECT 
                                        RedeSocialId ,
                                       Nome ,
                                       Url ,
                                       Ativo ,
                                       UsuarioInclusaoId ,
                                       DataInclusao ,
                                       NaturezaOperacao ,
                                       UsuarioOperacaoId ,
                                       DataOperacao
                                     FROM RedeSocial
                                     WHERE Ativo = 1
                                     ORDER BY Nome";

                return await _connection.QueryAsync<RedeSocial>(sql, commandType: CommandType.Text);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao obter redes sociais");
                throw new OperacaoRedeSocialException("consulta completa", ex);
            }
        }

        /// <summary>
        /// Obtém uma rede social específica pelo seu ID.
        /// </summary>
        /// <param name="redeSocialId">ID da rede social a ser buscada.</param>
        /// <returns>A rede social encontrada, ou <c>null</c> se não existir.</returns>
        /// <exception cref="OperacaoRedeSocialException">Lançada em caso de erro no banco de dados.</exception>
        public async Task<RedeSocial?> ObterRedeSocialPorIdAsync(int redeSocialId)
        {
            try
            {
                const string sql = @"SELECT 
                                        RedeSocialId ,
                                       Nome ,
                                       Url ,
                                       Ativo ,
                                       UsuarioInclusaoId ,
                                       DataInclusao ,
                                       NaturezaOperacao ,
                                       UsuarioOperacaoId ,
                                       DataOperacao
                                     FROM RedeSocial
                                     WHERE RedeSocialId = @redeSocialId AND Ativo = 1";

                var redeSocial = await _connection.QueryFirstOrDefaultAsync<RedeSocial>(sql, new { redeSocialId });

                if (redeSocial == null)
                {
                    _logger.LogWarning("RedeSocial com ID {RedeSocialId} não encontrada", redeSocialId);
                }

                return redeSocial;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro ao buscar rede social por ID: {RedeSocialId}", redeSocialId);
                throw new OperacaoRedeSocialException("busca por ID", ex);
            }
        }
    }
}