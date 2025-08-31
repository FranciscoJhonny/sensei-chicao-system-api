using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;

namespace TorneioSC.WebApi.Controllers
{
    /// <summary>
    /// Controlador REST para operações relacionadas à entidade RedeSocial.
    /// Fornece endpoints para listar redes sociais (ex: Instagram, WhatsApp, Facebook).
    /// </summary>
    [Route("api/redesocial")]
    [ApiController]
    public class RedeSocialController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRedeSocialService _redeSocialService;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="RedeSocialController"/>.
        /// </summary>
        /// <param name="mapper">Instância do AutoMapper para mapeamento entre modelos e DTOs.</param>
        /// <param name="redeSocialService">Serviço de negócios para operações com redes sociais.</param>
        /// <param name="loggerFactory">Fábrica de loggers (mantida para compatibilidade com outros controllers).</param>
        public RedeSocialController(
            IMapper mapper,
            IRedeSocialService redeSocialService,
            ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redeSocialService = redeSocialService ?? throw new ArgumentNullException(nameof(redeSocialService));
        }

        /// <summary>
        /// Obtém a lista de todas as redes sociais ativas do sistema.
        /// </summary>
        /// <returns>
        /// Uma lista de <see cref="RedeSocialDto"/> com ID, nome, logo e status.
        /// Exemplos comuns: Instagram, WhatsApp, Facebook, YouTube.
        /// </returns>
        /// <response code="200">Retorna a lista de redes sociais com sucesso.</response>
        /// <response code="404">Retorna se não houver redes sociais cadastradas.</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("get-lista-redesocial")]
        [ProducesResponseType(typeof(IEnumerable<RedeSocialDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRedeSocialAsync()
        {
            var redes = await _redeSocialService.ObterRedeSocialAsync();

            if (redes == null || !redes.Any())
                return NotFound(new { message = "Nenhuma rede social encontrada." });

            var response = _mapper.Map<IEnumerable<RedeSocialDto>>(redes);

            return Ok(response);
        }

        /// <summary>
        /// Obtém uma rede social específica pelo seu ID.
        /// </summary>
        /// <param name="redeSocialId">ID da rede social a ser recuperada.</param>
        /// <returns>
        /// Um <see cref="RedeSocialDto"/> com os dados da rede social, ou 404 se não encontrada.
        /// </returns>
        /// <response code="200">Retorna a rede social com sucesso.</response>
        /// <response code="400">Retorna se o ID for inválido (menor ou igual a zero).</response>
        /// <response code="404">Retorna se a rede social não for encontrada.</response>
        /// <response code="500">Retorna erro interno do servidor em caso de falha.</response>
        [HttpGet("por-id/{redeSocialId}")]
        [ProducesResponseType(typeof(RedeSocialDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRedeSocialPorIdAsync(int redeSocialId)
        {
            if (redeSocialId <= 0)
                return BadRequest(new { message = "ID da rede social inválido." });

            var rede = await _redeSocialService.ObterRedeSocialPorIdAsync(redeSocialId);

            if (rede == null)
                return NotFound(new { message = $"Rede social com ID {redeSocialId} não encontrada." });

            var response = _mapper.Map<RedeSocialDto>(rede);

            return Ok(response);
        }
    }
}