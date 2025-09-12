namespace TorneioSC.Domain.Dtos
{
    /// <summary>
    /// DTO que representa um resumo de evento para listagens.
    /// </summary>
    public class EventoResumo
    {
        public int EventoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Local { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string Responsavel { get; set; } = string.Empty;
        public int QuantidadeTorneios { get; set; }
    }
}