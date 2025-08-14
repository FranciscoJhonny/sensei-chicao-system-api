using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.MunicipioDtos;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/municipio")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly IMapper _mapper;        
        private readonly IMunicipioService _municipioService;

        public MunicipioController(IMapper mapper, IMunicipioService municipioService, ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));            
            _municipioService = municipioService ?? throw new ArgumentNullException(nameof(municipioService));
        }
        [HttpGet("get-lista-municipio")]
        [ProducesResponseType(typeof(MunicipioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMunicipioAsync()
        {
            var municipio = await _municipioService.ObterMunicipioAsync();

            var response = _mapper.Map<IEnumerable<MunicipioDto>>(municipio);

            if (response == null)
                return NotFound();

            return Ok(response);

        }

        [HttpGet("get-lista-municipio-estado/estadoId")]
        [ProducesResponseType(typeof(MunicipioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMunicipioPorEstadoIdAsync(int estadoId)
        {

            var municipio = await _municipioService.ObterMunicipioPorEstadoIdAsync(estadoId);

            var response = _mapper.Map<IEnumerable<MunicipioDto>>(municipio);

            if (response == null)
                return NotFound();

            return Ok(response);

        }
    }
}
