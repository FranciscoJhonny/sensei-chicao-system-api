namespace TorneioSC.WebApi.Dtos.EventoDtos
{
    /// <summary>
    /// DTO para filtros de consulta de eventos
    /// </summary>
    public class FiltroEventoDto
    {
        /// <summary>
        /// Nome do evento (busca parcial)
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Data de início mínima do evento
        /// </summary>
        public DateTime? DataInicioDe { get; set; }

        /// <summary>
        /// Data de início máxima do evento
        /// </summary>
        public DateTime? DataInicioAte { get; set; }

        /// <summary>
        /// Data de término mínima do evento
        /// </summary>
        public DateTime? DataFimDe { get; set; }

        /// <summary>
        /// Data de término máxima do evento
        /// </summary>
        public DateTime? DataFimAte { get; set; }

        /// <summary>
        /// Local do evento (busca parcial)
        /// </summary>
        public string? Local { get; set; }

        /// <summary>
        /// Nome do responsável pelo evento (busca parcial)
        /// </summary>
        public string? Responsavel { get; set; }

        /// <summary>
        /// Status do evento (true=ativo, false=inativo, null=todos)
        /// </summary>
        public bool? Ativo { get; set; } = true;

        /// <summary>
        /// ID do município para filtrar eventos por localização
        /// </summary>
        public int? MunicipioId { get; set; }

        /// <summary>
        /// ID da modalidade para filtrar eventos por tipo de competição
        /// </summary>
        public int? ModalidadeId { get; set; }

        /// <summary>
        /// Número da página para paginação (padrão: 1)
        /// </summary>
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Quantidade de registros por página (padrão: 10)
        /// </summary>
        public int TamanhoPagina { get; set; } = 10;
    }
}