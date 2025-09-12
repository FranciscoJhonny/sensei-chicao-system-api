// Arquivo: EventoService.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Dtos;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionEvento;

namespace TorneioSC.Application.Services
{
    /// <summary>
    /// Serviço responsável por gerenciar as operações de negócio relacionadas a eventos,
    /// como criação, atualização, inativação e consultas.
    /// </summary>
    public class EventoService : IEventoService
    {
        private readonly IEventoSqlWriteAdapter _eventoSqlWriteAdapter;
        private readonly IEventoSqlReadAdapter _eventoSqlReadAdapter;
        private readonly ILogger<EventoService> _logger;

        /// <summary>
        /// Inicializa uma nova instância do serviço de eventos.
        /// </summary>
        /// <param name="eventoSqlWriteAdapter">Adaptador de escrita para operações de persistência.</param>
        /// <param name="eventoSqlReadAdapter">Adaptador de leitura para operações de consulta.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public EventoService(
            IEventoSqlWriteAdapter eventoSqlWriteAdapter,
            IEventoSqlReadAdapter eventoSqlReadAdapter,
            ILogger<EventoService> logger)
        {
            _eventoSqlWriteAdapter = eventoSqlWriteAdapter ?? throw new ArgumentNullException(nameof(eventoSqlWriteAdapter));
            _eventoSqlReadAdapter = eventoSqlReadAdapter ?? throw new ArgumentNullException(nameof(eventoSqlReadAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region 🔽 Métodos de Escrita

        /// <inheritdoc />
        public async Task<int> PostEventoAsync(Evento evento)
        {
            _logger.LogInformation("Criando novo evento: {Nome}", evento.NomeEvento);

            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            // Validações básicas
            if (string.IsNullOrWhiteSpace(evento.NomeEvento))
                throw new ValidacaoEventoException("O nome do evento é obrigatório.");

            if (evento.DataInicio > evento.DataFim)
                throw new ValidacaoEventoException("A data de início não pode ser posterior à data de fim.");

            try
            {
                var id = await _eventoSqlWriteAdapter.PostEventoAsync(evento);
                _logger.LogInformation("Evento criado com sucesso. ID: {Id}", id);
                return id;
            }
            catch (OperacaoEventoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar evento: {Nome}", evento.NomeEvento);
                throw new OperacaoEventoException("criar", ex);
            }
        }

        /// <inheritdoc />
        public async Task<int> PutEventoAsync(Evento evento)
        {
            _logger.LogInformation("Atualizando evento ID: {EventoId}", evento.EventoId);

            if (evento == null)
                throw new ArgumentNullException(nameof(evento));

            if (string.IsNullOrWhiteSpace(evento.NomeEvento))
                throw new ValidacaoEventoException("O nome do evento é obrigatório.");

            if (evento.DataInicio > evento.DataFim)
                throw new ValidacaoEventoException("A data de início não pode ser posterior à data de fim.");

            try
            {
                var id = await _eventoSqlWriteAdapter.PutEventoAsync(evento);
                _logger.LogInformation("Evento ID {EventoId} atualizado com sucesso", evento.EventoId);
                return id;
            }
            catch (AtualizacaoEventoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar evento ID: {EventoId}", evento.EventoId);
                throw new AtualizacaoEventoException(evento.EventoId, ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> InativarEventoAsync(int eventoId, int usuarioOperacaoId)
        {
            _logger.LogInformation("Inativando evento ID: {EventoId}", eventoId);

            if (eventoId <= 0)
                throw new ArgumentException("ID do evento inválido.", nameof(eventoId));

            if (usuarioOperacaoId <= 0)
                throw new ArgumentException("ID do usuário operador inválido.", nameof(usuarioOperacaoId));

            try
            {
                var inativado = await _eventoSqlWriteAdapter.InativarEventoPorIdAsync(eventoId, usuarioOperacaoId);

                if (inativado)
                    _logger.LogInformation("Evento ID {EventoId} inativado com sucesso", eventoId);
                else
                    _logger.LogWarning("Evento ID {EventoId} não encontrado", eventoId);

                return inativado;
            }
            catch (ExclusaoEventoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar evento ID: {EventoId}", eventoId);
                throw new ExclusaoEventoException(eventoId, ex);
            }
        }

        #endregion

        #region 🔽 Métodos de Leitura

        /// <inheritdoc />
        public async Task<Evento?> ObterEventoPorIdAsync(int eventoId)
        {
            _logger.LogInformation("Buscando evento por ID: {EventoId}", eventoId);

            try
            {
                if (eventoId <= 0)
                {
                    _logger.LogWarning("ID de evento inválido: {EventoId}", eventoId);
                    throw new ArgumentException("ID do evento deve ser maior que zero", nameof(eventoId));
                }

                var evento = await _eventoSqlReadAdapter.ObterEventoPorIdAsync(eventoId);

                if (evento == null)
                {
                    _logger.LogInformation("Evento não encontrado para o ID: {EventoId}", eventoId);
                    return null;
                }

                _logger.LogInformation("Evento encontrado: {Nome} (ID: {EventoId})", evento.NomeEvento, eventoId);
                return evento;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erro de banco de dados ao buscar evento por ID: {EventoId}", eventoId);
                throw new OperacaoEventoException($"buscar evento com ID {eventoId}", ex);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar evento por ID: {EventoId}", eventoId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Evento>> ObterEventosAsync()
        {
            _logger.LogInformation("Buscando todos os eventos ativos");

            try
            {
                var eventos = await _eventoSqlReadAdapter.ObterEventosAsync();
                _logger.LogInformation("Encontrados {Quantidade} eventos", eventos?.Count() ?? 0);
                return eventos ?? Enumerable.Empty<Evento>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os eventos");
                throw new OperacaoEventoException("buscar todos os eventos", ex);
            }
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Evento> Eventos, int Total)> ObterEventosPorFiltroAsync(FiltroEvento filtro)
        {
            _logger.LogInformation("Buscando eventos por filtro: {@Filtro}", filtro);

            try
            {
                if (filtro.Pagina <= 0) filtro.Pagina = 1;
                if (filtro.TamanhoPagina <= 0 || filtro.TamanhoPagina > 100) filtro.TamanhoPagina = 10;

                var resultado = await _eventoSqlReadAdapter.ObterEventosPorFiltroAsync(filtro);

                _logger.LogInformation("Encontrados {Total} eventos, página {Pagina}", resultado.Total, filtro.Pagina);
                return resultado;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar eventos com filtros: {@Filtro}", filtro);
                throw new OperacaoEventoException("buscar eventos com filtros", ex);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync()
        {
            _logger.LogInformation("Buscando resumo de todos os eventos");

            try
            {
                return await _eventoSqlReadAdapter.ObterResumoEventosAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo dos eventos");
                throw new OperacaoEventoException("obtenção do resumo", ex);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventoResumo>> ObterResumoEventosAsync(FiltroEvento filtro)
        {
            _logger.LogInformation("Buscando resumo de eventos com filtros: {@Filtro}", filtro);

            try
            {
                return await _eventoSqlReadAdapter.ObterResumoEventosAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo dos eventos com filtros: {@Filtro}", filtro);
                throw new OperacaoEventoException("obtenção do resumo com filtros", ex);
            }
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<EventoResumo> Resumos, int Total)> ObterResumoEventosPaginadoAsync(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

            _logger.LogInformation("Buscando resumo de eventos paginado: página {Pagina}, tamanho {TamanhoPagina}", pagina, tamanhoPagina);

            try
            {
                return await _eventoSqlReadAdapter.ObterResumoEventosPaginadoAsync(pagina, tamanhoPagina);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter resumo paginado dos eventos");
                throw new OperacaoEventoException("obtenção do resumo paginado", ex);
            }
        }

        /// <inheritdoc />
        public async Task<int> ObterTotalEventosAtivosAsync()
        {
            _logger.LogInformation("Buscando total de eventos ativos");

            try
            {
                return await _eventoSqlReadAdapter.ObterTotalEventosAtivosAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao contar eventos ativos");
                throw new OperacaoEventoException("contagem de eventos ativos", ex);
            }
        }

        /// <inheritdoc />
        public async Task<int> ObterTotalEventosAsync(FiltroEvento? filtro = null)
        {
            _logger.LogInformation("Contando eventos com filtros: {@Filtro}", filtro);

            try
            {
                return await _eventoSqlReadAdapter.ObterTotalEventosAsync(filtro);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao contar eventos com filtros: {@Filtro}", filtro);
                throw new OperacaoEventoException("contagem de eventos", ex);
            }
        }

        /// <inheritdoc />
        public async Task<EstatisticasEventos> ObterEstatisticasEventosAsync()
        {
            _logger.LogInformation("Buscando estatísticas dos eventos");

            try
            {
                return await _eventoSqlReadAdapter.ObterEstatisticasEventosAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter estatísticas dos eventos");
                throw new OperacaoEventoException("obtenção de estatísticas", ex);
            }
        }

        #endregion
    }
}