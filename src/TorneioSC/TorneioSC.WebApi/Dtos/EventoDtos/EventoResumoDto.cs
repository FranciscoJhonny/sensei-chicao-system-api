namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO que representa um resumo de evento para listagens e relatórios
    /// </summary>
    public class EventoResumoDto
    {
        /// <summary>
        /// ID único do evento
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Nome do evento
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de início do evento
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término do evento
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Local onde o evento será realizado
        /// </summary>
        public string Local { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pelo evento
        /// </summary>
        public string Responsavel { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o evento está ativo no sistema
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Quantidade total de torneios associados ao evento
        /// </summary>
        public int QuantidadeTorneios { get; set; }
    }
}