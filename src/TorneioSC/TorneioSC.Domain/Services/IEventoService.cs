using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;

namespace TorneioSC.Domain.Services
{
    /// <summary>
    /// Define os serviços de negócio para operações relacionadas a eventos,
    /// incluindo criação, atualização, inativação e consultas.
    /// </summary>
    public interface IEventoService
    {
        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Cria um novo evento juntamente com seu torneio e respectivas categorias.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados do evento, torneio e categorias a serem criados.</param>
        /// <returns>O ID do evento recém-criado.</returns>
        Task<int> PostEventoAsync(Evento evento);

        /// <summary>
        /// Atualiza um evento existente, incluindo seu torneio e categorias associadas.
        /// </summary>
        /// <param name="evento">Objeto contendo os dados atualizados do evento. Deve incluir o <see cref="Evento.EventoId"/>.</param>
        /// <returns>O ID do evento atualizado.</returns>
        Task<int> PutEventoAsync(Evento evento);

        /// <summary>
        /// Inativa logicamente um evento (marca como inativo) com base no seu ID.
        /// </summary>
        /// <param name="eventoId">ID do evento a ser inativado.</param>
        /// <param name="usuarioOperacaoId">ID do usuário que está realizando a operação de inativação.</param>
        /// <returns>Retorna <c>true</c> se a inativação for bem-sucedida.</returns>
        Task<bool> InativarEventoAsync(int eventoId, int usuarioOperacaoId);

        #endregion

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém um evento específico pelo seu ID, incluindo torneio e categorias.
        /// </summary>
        /// <param name="eventoId">ID do evento a ser recuperado.</param>
        /// <returns>O evento se encontrado; caso contrário, retorna null.</returns>
        Task<Evento?> ObterEventoPorIdAsync(int eventoId);

        /// <summary>
        /// Obtém todos os eventos ativos do sistema.
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
        /// Obtém um resumo dos eventos com base em filtros.
        /// </summary>
        /// <param name="filtro">Filtros para a consulta.</param>
        /// <returns>Lista de objetos <see cref="EventoResumo"/> que atendem ao filtro.</returns>
        Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync(FiltroEvento filtro);

        /// <summary>
        /// Obtém um resumo dos eventos com paginação.
        /// </summary>
        /// <param name="pagina">Número da página (começa em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página.</param>
        /// <returns>Tupla contendo a lista de resumos e o total de registros.</returns>
        Task<(IEnumerable<EventoResumo> Resumos, int Total)> ObterResumoEventosPaginadoAsync(int pagina = 1, int tamanhoPagina = 10);

        /// <summary>
        /// Obtém o total de eventos ativos no sistema.
        /// </summary>
        /// <returns>Número total de eventos ativos.</returns>
        Task<int> ObterTotalEventosAtivosAsync();

        /// <summary>
        /// Obtém o total de eventos com base em filtros opcionais.
        /// </summary>
        /// <param name="filtro">Filtros opcionais para contagem.</param>
        /// <returns>Número total de eventos que correspondem ao filtro.</returns>
        Task<int> ObterTotalEventosAsync(FiltroEvento? filtro = null);

        /// <summary>
        /// Obtém estatísticas gerais sobre eventos (total, ativos, inativos, etc).
        /// </summary>
        /// <returns>Objeto <see cref="EstatisticasEventos"/> com as métricas.</returns>
        Task<EstatisticasEventos> ObterEstatisticasEventosAsync();

        #endregion
    }
}