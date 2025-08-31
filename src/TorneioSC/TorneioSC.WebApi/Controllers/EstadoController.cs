using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.EstadoDtos;

namespace TorneioSC.WebApi.Controllers
{
    /// <summary>
    /// Controlador REST para operações relacionadas à entidade Estado.
    /// Fornece endpoints para listar estados e obter detalhes de um estado específico com seus municípios.
    /// </summary>
    [Route("api/estado")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEstadoService _estadoService;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="EstadoController"/>.
        /// </summary>
        /// <param name="mapper">Instância do AutoMapper para mapeamento entre modelos e DTOs.</param>
        /// <param name="estadoService">Serviço de negócios para operações com estados.</param>
        /// <param name="loggerFactory">Fábrica de loggers (não utilizada diretamente, mas mantida para compatibilidade).</param>
        public EstadoController(IMapper mapper, IEstadoService estadoService, ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _estadoService = estadoService ?? throw new ArgumentNullException(nameof(estadoService));
        }

        /// <summary>
        /// Obtém a lista de todos os estados ativos do sistema.
        /// </summary>
        /// <returns>
        /// Uma lista de <see cref="EstadoDto"/> com ID, nome e sigla de todos os estados.
        /// </returns>
        /// <response code="200">Retorna a lista de estados com sucesso.</response>
        /// <response code="404">Retorna 404 se não houver estados cadastrados (embora retorne lista vazia, não nula).</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("get-lista-estado")]
        [ProducesResponseType(typeof(IEnumerable<EstadoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstadoAsync()
        {
            var estados = await _estadoService.ObterEstadoAsync();

            if (estados == null || !estados.Any())
                return NotFound(new { message = "Nenhum estado encontrado." });

            var response = _mapper.Map<IEnumerable<EstadoDto>>(estados);

            return Ok(response);
        }

        /// <summary>
        /// Obtém um estado específico pelo seu ID, incluindo os municípios associados.
        /// </summary>
        /// <param name="estadoId">ID do estado a ser recuperado.</param>
        /// <returns>
        /// Um <see cref="EstadoDto"/> contendo o estado e seus municípios, ou 404 se não encontrado.
        /// </returns>
        /// <response code="200">Retorna o estado com sucesso.</response>
        /// <response code="400">Retorna se o ID for inválido (menor ou igual a zero).</response>
        /// <response code="404">Retorna se o estado não for encontrado.</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("por-id/{estadoId}")]
        [ProducesResponseType(typeof(EstadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstadoPorIdAsync(int estadoId)
        {
            if (estadoId <= 0)
                return BadRequest(new { message = "ID do estado inválido." });

            var estado = await _estadoService.ObterEstadoPorIdAsync(estadoId);

            if (estado == null)
                return NotFound(new { message = $"Estado com ID {estadoId} não encontrado." });

            var response = _mapper.Map<EstadoDto>(estado);

            return Ok(response);
        }
    }
}