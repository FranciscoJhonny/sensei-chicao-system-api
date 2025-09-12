// Arquivo: EventoController.cs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Application.Services;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionEvento;
using TorneioSC.WebApi.Dtos.EventoDtos;
using System;
using System.Threading.Tasks;

namespace TorneioSC.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações de cadastro, consulta, atualização e inativação de eventos.
    /// </summary>
    [ApiController]
    [Route("api/evento")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa uma nova instância do controlador de evento.
        /// </summary>
        /// <param name="mapper">Instância do AutoMapper para mapeamento de objetos.</param>
        /// <param name="eventoService">Serviço de negócio para operações com eventos.</param>
        public EventoController(IMapper mapper, IEventoService eventoService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventoService = eventoService ?? throw new ArgumentNullException(nameof(eventoService));
        }

        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Cria um novo evento com seu torneio associado, incluindo categorias.
        /// </summary>
        /// <param name="eventoDto">Objeto contendo os dados do evento e torneio a serem criados.</param>
        /// <returns>O ID do evento criado.</returns>
        /// <response code="201">Retorna o ID do evento criado e o caminho para o recurso.</response>
        /// <response code="400">Retorna uma mensagem de erro de validação se os dados forem inválidos.</response>
        /// <response code="500">Retorna um erro interno do servidor em caso de falha na operação.</response>
        [HttpPost("post-torneio")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostEventoAsync([FromBody] EventoPostDto eventoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var evento = _mapper.Map<Evento>(eventoDto);
                var id = await _eventoService.PostEventoAsync(evento);

                var eventoCriado = await _eventoService.ObterEventoPorIdAsync(id);
                var eventoRetorno = _mapper.Map<EventoDto>(eventoCriado);

                return CreatedAtRoute("ObterEventoPorId", new { id }, eventoRetorno);
            }
            catch (ValidacaoEventoException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (OperacaoEventoException ex)
            {
                return StatusCode(500, new { error = "Erro ao criar evento.", details = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um evento existente, incluindo seu torneio e categorias associadas.
        /// </summary>
        /// <param name="eventoDto">Objeto contendo os dados atualizados do evento.</param>
        /// <returns>Dados do evento atualizado.</returns>
        /// <response code="200">Retorna os dados do evento atualizado.</response>
        /// <response code="400">Retorna erro de validação.</response>
        /// <response code="404">Retorna se o evento não for encontrado.</response>
        /// <response code="500">Retorna erro interno.</response>
        [HttpPut("put-torneio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEventoAsync([FromBody] EventoPutDto eventoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var evento = _mapper.Map<Evento>(eventoDto);
                await _eventoService.PutEventoAsync(evento);

                var eventoAtualizado = await _eventoService.ObterEventoPorIdAsync(evento.EventoId);
                var eventoRetorno = _mapper.Map<EventoDto>(eventoAtualizado);

                return Ok(eventoRetorno);
            }
            catch (ValidacaoEventoException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (AtualizacaoEventoException ex)
            {
                return NotFound(new { error = $"Evento {ex.EventoId} não encontrado." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao atualizar evento.", details = ex.Message });
            }
        }

        /// <summary>
        /// Inativa um evento (desativa logicamente) com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do evento a ser inativado.</param>
        /// <param name="usuarioId">ID do usuário que está realizando a operação.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        /// <response code="200">Retorna mensagem de sucesso.</response>
        /// <response code="400">Retorna erro se os parâmetros forem inválidos.</response>
        /// <response code="404">Retorna se o evento não for encontrado.</response>
        /// <response code="500">Retorna erro interno.</response>
        [HttpPut("inativar-torneio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InativarEventoAsync(int id, [FromQuery] int usuarioId)
        {
            if (id <= 0)
                return BadRequest("ID do evento inválido.");

            if (usuarioId <= 0)
                return BadRequest("ID do usuário inválido.");

            try
            {
                var evento = await _eventoService.ObterEventoPorIdAsync(id);
                if (evento == null)
                    return NotFound(new { error = $"Evento {id} não encontrado." });

                var sucesso = await _eventoService.InativarEventoAsync(id, usuarioId);

                return Ok(new
                {
                    success = sucesso,
                    message = sucesso ? "Evento inativado com sucesso." : "Falha ao inativar evento."
                });
            }
            catch (ExclusaoEventoException ex)
            {
                return NotFound(new { error = $"Evento {ex.EventoId} não encontrado." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao inativar evento.", details = ex.Message });
            }
        }

        #endregion

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém um evento específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do evento.</param>
        /// <returns>Dados completos do evento.</returns>
        /// <response code="200">Retorna o evento.</response>
        /// <response code="404">Retorna se não encontrado.</response>
        [HttpGet("{id}", Name = "ObterEventoPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            if (id <= 0)
                return BadRequest("ID do evento inválido.");

            try
            {
                var evento = await _eventoService.ObterEventoPorIdAsync(id);
                if (evento == null)
                    return NotFound(new { message = $"Evento com ID {id} não encontrado." });

                return Ok(_mapper.Map<EventoDto>(evento));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar evento.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém todos os eventos ativos do sistema.
        /// </summary>
        /// <returns>Lista de eventos ativos.</returns>
        /// <response code="200">Retorna a lista de eventos.</response>
        [HttpGet("todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var eventos = await _eventoService.ObterEventosAsync();
                return Ok(_mapper.Map<IEnumerable<EventoDto>>(eventos));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar eventos.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém eventos com base em filtros, com paginação.
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis (nome, datas, local, ativo, etc).</param>
        /// <returns>Lista paginada de eventos e total de registros.</returns>
        /// <response code="200">Retorna eventos e total.</response>
        /// <response code="400">Retorna erro se os parâmetros forem inválidos.</response>
        [HttpGet("filtro")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ObterPorFiltro([FromQuery] FiltroEventoDto filtro)
        {
            try
            {
                var filtroModel = _mapper.Map<FiltroEvento>(filtro);
                var resultado = await _eventoService.ObterEventosPorFiltroAsync(filtroModel);

                var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(resultado.Eventos);

                return Ok(new
                {
                    Data = eventosDto,
                    Total = resultado.Total,
                    Pagina = filtro.Pagina,
                    TamanhoPagina = filtro.TamanhoPagina,
                    TotalPaginas = (int)Math.Ceiling((double)resultado.Total / filtro.TamanhoPagina)
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar eventos com filtros.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um resumo de todos os eventos (dados essenciais).
        /// </summary>
        /// <returns>Lista de resumos de eventos.</returns>
        /// <response code="200">Retorna os resumos.</response>
        [HttpGet("resumo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumo()
        {
            try
            {
                var resumos = await _eventoService.ObterResumoEventosAsync();
                return Ok(resumos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao obter resumo dos eventos.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um resumo dos eventos com paginação.
        /// </summary>
        /// <param name="pagina">Página atual (padrão: 1).</param>
        /// <param name="tamanhoPagina">Quantidade por página (máx: 100).</param>
        /// <returns>Lista paginada de resumos e total.</returns>
        /// <response code="200">Retorna os resumos paginados.</response>
        [HttpGet("resumo-paginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

            try
            {
                var (resumos, total) = await _eventoService.ObterResumoEventosPaginadoAsync(pagina, tamanhoPagina);

                return Ok(new
                {
                    Resumos = resumos,
                    Pagina = pagina,
                    TamanhoPagina = tamanhoPagina,
                    Total = total,
                    TotalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina)
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao obter resumo paginado.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um resumo dos eventos com base em filtros.
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis.</param>
        /// <returns>Lista de resumos filtrados.</returns>
        /// <response code="200">Retorna os resumos filtrados.</response>
        [HttpGet("resumo-filtrado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoFiltrado([FromQuery] FiltroEventoDto filtro)
        {
            try
            {
                var filtroModel = _mapper.Map<FiltroEvento>(filtro);
                var resumos = await _eventoService.ObterResumoEventosAsync(filtroModel);
                return Ok(resumos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao obter resumo filtrado.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o total de eventos com base em filtros opcionais.
        /// </summary>
        /// <param name="filtro">Filtros (Nome, Data, Local, Ativo, etc).</param>
        /// <returns>Número total de eventos.</returns>
        /// <response code="200">Retorna o total.</response>
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTotal([FromQuery] FiltroEventoDto filtro)
        {
            try
            {
                var filtroModel = _mapper.Map<FiltroEvento>(filtro);
                var total = await _eventoService.ObterTotalEventosAsync(filtroModel);
                return Ok(new { Total = total });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao contar eventos.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o total de eventos ativos no sistema.
        /// </summary>
        /// <returns>Quantidade de eventos ativos.</returns>
        /// <response code="200">Retorna o total de ativos.</response>
        [HttpGet("total-ativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTotalAtivos()
        {
            try
            {
                var total = await _eventoService.ObterTotalEventosAtivosAsync();
                return Ok(new { TotalAtivos = total });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao contar eventos ativos.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém estatísticas gerais dos eventos (total, ativos, inativos, status por data).
        /// </summary>
        /// <returns>Estatísticas detalhadas.</returns>
        /// <response code="200">Retorna as estatísticas.</response>
        [HttpGet("estatisticas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterEstatisticas()
        {
            try
            {
                var estatisticas = await _eventoService.ObterEstatisticasEventosAsync();
                return Ok(estatisticas);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao obter estatísticas dos eventos.", details = ex.Message });
            }
        }

        /// <summary>
        /// Verifica se já existe um evento com o mesmo nome e período (para evitar conflitos).
        /// </summary>
        /// <param name="nome">Nome do evento.</param>
        /// <param name="dataInicio">Data de início.</param>
        /// <param name="dataFim">Data de fim.</param>
        /// <param name="eventoId">ID do evento sendo editado (opcional, para exclusão na verificação).</param>
        /// <returns>Evento conflitante ou null.</returns>
        /// <response code="200">Retorna o evento conflitante, se houver.</response>
        [HttpGet("validar-conflito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidarConflito(
            [FromQuery] string nome,
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim,
            [FromQuery] int? eventoId = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome é obrigatório.");

            if (dataInicio > dataFim)
                return BadRequest("Data de início não pode ser posterior à data de fim.");

            try
            {
                var filtro = new FiltroEvento
                {
                    Nome = nome,
                    DataInicioDe = dataInicio.AddDays(-1),
                    DataInicioAte = dataFim.AddDays(1),
                    Ativo = true
                };

                var eventos = await _eventoService.ObterEventosPorFiltroAsync(filtro);

                var conflito = eventos.Eventos
                    .FirstOrDefault(e => e.EventoId != eventoId);

                if (conflito == null)
                    return Ok(new { Conflito = false });

                return Ok(new
                {
                    Conflito = true,
                    EventoId = conflito.EventoId,
                    Nome = conflito.NomeEvento,
                    DataInicio = conflito.DataInicio,
                    DataFim = conflito.DataFim,
                    Local = conflito.Local
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao validar conflito de evento.", details = ex.Message });
            }
        }

        #endregion
    }
}