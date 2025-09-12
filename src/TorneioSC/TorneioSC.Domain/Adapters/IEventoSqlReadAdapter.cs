using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Adapters
{
    /// <summary>
    /// Adaptador de leitura para operações de consulta relacionadas a eventos.
    /// </summary>
    public interface IEventoSqlReadAdapter
    {
        /// <summary>
        /// Obtém o total de eventos ativos no sistema.
        /// </summary>
        Task<int> ObterTotalEventosAtivosAsync();

        /// <summary>
        /// Obtém o total de eventos com base em filtros opcionais.
        /// </summary>
        /// <param name="filtro">Filtros opcionais para contagem (ex: nome, data, ativo).</param>
        /// <returns>Número total de eventos que correspondem ao filtro.</returns>
        Task<int> ObterTotalEventosAsync(FiltroEvento? filtro = null);

        /// <summary>
        /// Obtém um evento específico pelo seu ID.
        /// </summary>
        /// <param name="eventoId">ID do evento a ser recuperado.</param>
        /// <returns>O evento se encontrado; caso contrário, retorna null.</returns>
        Task<Evento?> ObterEventoPorIdAsync(int eventoId);

        /// <summary>
        /// Obtém todos os eventos ativos.
        /// </summary>
        /// <returns>Lista de eventos ativos.</returns>
        Task<IEnumerable<Evento>> ObterEventosAsync();

        /// <summary>
        /// Obtém eventos com base em filtros, com paginação.
        /// </summary>
        /// <param name="filtro">Objeto contendo critérios de filtro (nome, datas, ativo, etc).</param>
        /// <returns>Tupla contendo a lista de eventos e o total de registros (para paginação).</returns>
        Task<(IEnumerable<Evento> Eventos, int Total)> ObterEventosPorFiltroAsync(FiltroEvento filtro);

        /// <summary>
        /// Obtém um resumo dos eventos (dados essenciais para listagem).
        /// </summary>
        /// <returns>Lista de objetos <see cref="EventoResumo"/>.</returns>
        Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync();

        /// <summary>
        /// Obtém um resumo dos eventos com paginação.
        /// </summary>
        /// <param name="pagina">Número da página (começa em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página.</param>
        /// <returns>Tupla contendo a lista de resumos e o total de registros.</returns>
        Task<(IEnumerable<EventoResumo> Resumos, int Total)> ObterResumoEventosPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);

        /// <summary>
        /// Obtém um resumo dos eventos com base em filtros.
        /// </summary>
        /// <param name="filtro">Filtros para a consulta.</param>
        /// <returns>Lista de objetos <see cref="EventoResumo"/> que atendem ao filtro.</returns>
        Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync(FiltroEvento filtro);

        /// <summary>
        /// Obtém estatísticas gerais sobre eventos (total, ativos, inativos, etc).
        /// </summary>
        /// <returns>Objeto <see cref="EstatisticasEventos"/> com as métricas.</returns>
        Task<EstatisticasEventos> ObterEstatisticasEventosAsync();
    }
}