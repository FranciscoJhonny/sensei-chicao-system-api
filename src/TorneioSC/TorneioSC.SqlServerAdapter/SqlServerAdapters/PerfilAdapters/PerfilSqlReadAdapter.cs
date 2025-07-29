using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.UsuarioAdapters;

namespace TorneioSC.SqlServerAdapter.SqlServerAdapters.PerfilAdapters
{
    public class PerfilSqlReadAdapter : IPerfilSqlReadAdapter
    {
        //private readonly TorneioDbContext _dbContext;
        private readonly SqlConnection _connection; // Adicionei esta linha
        private readonly ILogger<UsuarioSqlReadAdapter> _logger;

        public PerfilSqlReadAdapter(//TorneioDbContext dbContext, 
            ILoggerFactory loggerFactory, 
            SqlAdapterContext context)
        {
            //_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _connection = context?.Connection ?? throw new ArgumentNullException(nameof(context));
            _logger = loggerFactory?.CreateLogger<UsuarioSqlReadAdapter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            // Verificação adicional se necessário
            if (_connection.State == ConnectionState.Closed)
            {
                _logger.LogWarning("Conexão recebida estava fechada");
            }
        }

        public async Task<Perfil?> ObterPerfilPorIdAsync(int perfilId)
        {
            if (perfilId <= 0)
                throw new ArgumentException("ID do perfil deve ser maior que zero", nameof(perfilId));

            const string sql = @"SELECT PerfilId,
                                Descricao,
                                Ativo,
                                DataInclusao,
                                DataOperacao 
                         FROM Perfil 
                         WHERE PerfilId = @perfilId";

            return await _connection.QueryFirstOrDefaultAsync<Perfil>(sql, new { perfilId });
        }
    }
}
