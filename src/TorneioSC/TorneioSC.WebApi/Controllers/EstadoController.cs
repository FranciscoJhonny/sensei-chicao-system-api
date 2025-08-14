using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.EstadoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/estado")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEstadoService _estadoService;


        public EstadoController(IMapper mapper, IEstadoService estadoService, ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _estadoService = estadoService ?? throw new ArgumentNullException(nameof(estadoService));
        }
        [HttpGet("get-lista-estado")]
        [ProducesResponseType(typeof(EstadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstadoAsync()
        {
            var estado = await _estadoService.ObterEstadoAsync();

            var response = _mapper.Map<IEnumerable<EstadoDto>>(estado);

            if (response == null)
                return NotFound();

            return Ok(response);

        }
        [HttpGet("get-lista-estado/estadoId")]
        [ProducesResponseType(typeof(EstadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstadoIdAsync(int estadoId)
        {

            var estado = await _estadoService.ObterEstadoPorIdAsync(estadoId);

            var response = _mapper.Map<EstadoDto>(estado);

            if (response == null)
                return NotFound();

            return Ok(response);

        }
    }
}
