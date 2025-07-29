namespace TorneioSC.SqlServerAdapter.Context
{

    public class SqlServerAdapterConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int CommandTimeout { get; set; } = 30;
        public bool EnableTransaction { get; set; } = true;
        // Outras propriedades de configuração conforme necessário
    }
}