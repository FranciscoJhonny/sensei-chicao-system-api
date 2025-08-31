using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.TipoTelefoneDtos;

namespace TorneioSC.WebApi.Controllers
{
    /// <summary>
    /// Controlador REST para operações relacionadas à entidade TipoTelefone.
    /// Fornece endpoints para listar tipos de telefone do sistema (ex: Celular, Comercial, Residencial).
    /// </summary>
    [Route("api/tipotelefone")]
    [ApiController]
    public class TipoTelefoneController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITipoTelefoneService _tipoTelefoneService;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="TipoTelefoneController"/>.
        /// </summary>
        /// <param name="mapper">Instância do AutoMapper para mapeamento entre modelos e DTOs.</param>
        /// <param name="tipoTelefoneService">Serviço de negócios para operações com tipos de telefone.</param>
        /// <param name="loggerFactory">Fábrica de loggers (mantida para compatibilidade com outros controllers).</param>
        public TipoTelefoneController(IMapper mapper,ITipoTelefoneService tipoTelefoneService,ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tipoTelefoneService = tipoTelefoneService ?? throw new ArgumentNullException(nameof(tipoTelefoneService));
        }

        /// <summary>
        /// Obtém a lista de todos os tipos de telefone ativos do sistema.
        /// </summary>
        /// <returns>
        /// Uma lista de <see cref="TipoTelefoneDto"/> com ID, descrição e status de ativação.
        /// Exemplos comuns: Celular, Comercial, Residencial.
        /// </returns>
        /// <response code="200">Retorna a lista de tipos de telefone com sucesso.</response>
        /// <response code="404">Retorna se não houver tipos de telefone cadastrados.</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("get-lista-tipotelefone")]
        [ProducesResponseType(typeof(IEnumerable<TipoTelefoneDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTipoTelefoneAsync()
        {
            var tipos = await _tipoTelefoneService.ObterTipoTelefoneAsync();

            if (tipos == null || !tipos.Any())
                return NotFound(new { message = "Nenhum tipo de telefone encontrado." });

            var response = _mapper.Map<IEnumerable<TipoTelefoneDto>>(tipos);

            return Ok(response);
        }

        /// <summary>
        /// Obtém um tipo de telefone específico pelo seu ID.
        /// </summary>
        /// <param name="tipoTelefoneId">ID do tipo de telefone a ser recuperado.</param>
        /// <returns>
        /// Um <see cref="TipoTelefoneDto"/> com os dados do tipo de telefone, ou 404 se não encontrado.
        /// </returns>
        /// <response code="200">Retorna o tipo de telefone com sucesso.</response>
        /// <response code="400">Retorna se o ID for inválido (menor ou igual a zero).</response>
        /// <response code="404">Retorna se o tipo de telefone não for encontrado.</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("por-id/{tipoTelefoneId}")]
        [ProducesResponseType(typeof(TipoTelefoneDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTipoTelefonePorIdAsync(int tipoTelefoneId)
        {
            if (tipoTelefoneId <= 0)
                return BadRequest(new { message = "ID do tipo de telefone inválido." });

            var tipo = await _tipoTelefoneService.ObterTipoTelefonePorIdAsync(tipoTelefoneId);

            if (tipo == null)
                return NotFound(new { message = $"Tipo de telefone com ID {tipoTelefoneId} não encontrado." });

            var response = _mapper.Map<TipoTelefoneDto>(tipo);

            return Ok(response);
        }
    }
}