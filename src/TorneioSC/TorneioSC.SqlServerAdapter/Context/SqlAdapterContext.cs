using Microsoft.Data.SqlClient;
using TorneioSC.SqlServerAdapter.Context;

public class SqlAdapterContext : IDisposable
{
    private readonly SqlConnection _connection;
    private readonly SqlServerAdapterConfiguration _config;

    public SqlConnection Connection => _connection;

    public SqlAdapterContext(SqlConnection connection, SqlServerAdapterConfiguration config)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public static SqlAdapterContext CreateFromConnectionString(string connectionString, int commandTimeout = 30)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be empty", nameof(connectionString));

        var connection = new SqlConnection(connectionString);
        var config = new SqlServerAdapterConfiguration
        {
            ConnectionString = connectionString,
            CommandTimeout = commandTimeout
        };

        return new SqlAdapterContext(connection, config);
    }

    public async Task OpenConnectionAsync()
    {
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }

    public async Task<SqlDataReader> ExecuteReaderAsync(string commandText, params SqlParameter[] parameters)
    {
        using (var command = new SqlCommand(commandText, _connection))
        {
            command.CommandTimeout = _config.CommandTimeout;
            command.Parameters.AddRange(parameters);
            return await command.ExecuteReaderAsync();
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}