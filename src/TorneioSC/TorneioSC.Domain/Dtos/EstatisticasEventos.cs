namespace TorneioSC.Domain.Dtos
{
    /// <summary>
    /// Representa estatísticas agregadas sobre eventos no sistema.
    /// </summary>
    public class EstatisticasEventos
    {
        public int Total { get; set; }
        public int Ativos { get; set; }
        public int Inativos { get; set; }
        public int Proximos { get; set; }
        public int EmAndamento { get; set; }
        public int Finalizados { get; set; }
    }
}
