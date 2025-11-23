using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.MunicipioDtos;

namespace TorneioSC.WebApi.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de municípios
    /// </summary>
    [Route("api/municipio")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMunicipioService _municipioService;

        /// <summary>
        /// Construtor do controller de municípios
        /// </summary>
        /// <param name="mapper">Mapper para conversão de DTOs</param>
        /// <param name="municipioService">Serviço de municípios</param>
        /// <param name="loggerFactory">Factory para criação de loggers</param>
        public MunicipioController(IMapper mapper, IMunicipioService municipioService, ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _municipioService = municipioService ?? throw new ArgumentNullException(nameof(municipioService));
        }

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém a lista de todos os municípios cadastrados
        /// </summary>
        /// <returns>Lista de municípios</returns>
        /// <response code="200">Retorna a lista de municípios</response>
        /// <response code="404">Nenhum município encontrado</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("get-lista-municipio")]
        [ProducesResponseType(typeof(IEnumerable<MunicipioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Obtém a lista de municípios por ID do estado
        /// </summary>
        /// <param name="estadoId">ID do estado para filtrar os municípios</param>
        /// <returns>Lista de municípios do estado especificado</returns>
        /// <response code="200">Retorna a lista de municípios do estado</response>
        /// <response code="404">Nenhum município encontrado para o estado</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("get-lista-municipio-estado/{estadoId}")]
        [ProducesResponseType(typeof(IEnumerable<MunicipioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        #endregion

        #region 🔽 Métodos de Escrita

        // Como a MunicipioController atual só tem métodos de leitura,
        // esta região ficará vazia até que métodos de escrita sejam adicionados
        // Exemplo futuro:
        // [HttpPost("criar-municipio")]
        // public async Task<IActionResult> PostMunicipioAsync([FromBody] CriarMunicipioDto municipioDto) { ... }

        // [HttpPut("atualizar-municipio/{id}")]
        // public async Task<IActionResult> PutMunicipioAsync(int id, [FromBody] AtualizarMunicipioDto municipioDto) { ... }

        // [HttpDelete("excluir-municipio/{id}")]
        // public async Task<IActionResult> DeleteMunicipioAsync(int id) { ... }

        #endregion

        #region 🔽 Validações

        // Como a MunicipioController atual só tem métodos de leitura,
        // esta região ficará vazia até que validações sejam necessárias
        // Exemplo futuro:
        // private bool ValidarMunicipioDto(CriarMunicipioDto municipioDto) { ... }
        // private bool ValidarId(int id) { ... }

        #endregion
    }
}