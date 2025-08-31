using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Application.Services;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionAcademia;
using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/academia")]
    [ApiController]
    public class AcademiaController : ControllerBase
    {
        private readonly IAcademiaService _academiaService;
        private readonly IMapper _mapper;

        public AcademiaController(IMapper mapper, IAcademiaService academiaService, ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _academiaService = academiaService ?? throw new ArgumentNullException(nameof(academiaService));
        }

        #region 🔽 Métodos de Escrita

        //[Authorize(Roles = "Administrador")]
        [HttpPost("post-academia")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostAcademia([FromBody] AcademiaPostDto academiaDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var academia = _mapper.Map<Academia>(academiaDto);
            var novaAcademiaId = await _academiaService.PostAcademiaAsync(academia);

            var academiaCriada = await _academiaService.ObterAcademiaPorIdAsync(novaAcademiaId);

            return CreatedAtRoute(
                "ObterAcademiaPorId",
                new { id = novaAcademiaId },
                _mapper.Map<AcademiaDto>(academiaCriada));
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("put-academia")]
        [ProducesResponseType(typeof(AcademiaPutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAcademia([FromBody] AcademiaPutDto academiaDto)
        {
            if (academiaDto is null)
                throw new ArgumentNullException(nameof(academiaDto));


            var academia = _mapper.Map<Academia>(academiaDto);
           
            var academiaAtualizadaId = await _academiaService.PutAcademiaAsync(academia);

            var academiaAtualizada = await _academiaService.ObterAcademiaPorIdAsync(academiaAtualizadaId);

            return Ok(_mapper.Map<AcademiaDto>(academiaAtualizada));
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("inativar-academia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InativarAcademia([FromBody] AcademiaInativacaoDto academiaDeleteDto)
        {
            var academia = await _academiaService.ObterAcademiaPorIdAsync(academiaDeleteDto.AcademiaId);

            if (academia == null)
                return NotFound();

            var result = await _academiaService.InativarAcademiaPorIdAsync(academiaDeleteDto.AcademiaId, academiaDeleteDto.UsuarioOperacaoId);

            return Ok(new { success = result, message = result ? "Academia intivada com sucesso" : "Falha ao inativar academia" });
        }

        #endregion

        #region 🔽 Métodos de leitura

        [HttpGet("get-por-cnpj")]
        [ProducesResponseType(typeof(AcademiaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorCnpjAsync([FromQuery] string cnpj)
        {
            var academia = await _academiaService.ObterPorCnpjAsync(cnpj);
            if (academia == null)
                return NotFound();

            return Ok(_mapper.Map<AcademiaDto>(academia));
        }

        //[Authorize]
        [HttpGet("get-por-id/{id}", Name = "ObterAcademiaPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var academia = await _academiaService.ObterAcademiaPorIdAsync(id);
            if (academia == null)
                return NotFound();

            return Ok(_mapper.Map<AcademiaDto>(academia));
        }

        //[Authorize]
        [HttpGet("filtro")]
        [ProducesResponseType(typeof(AcademiaFiltroResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterAcademiasPorFiltro([FromQuery] FiltroAcademia filtro)
        {
            try
            {
                var resultado = await _academiaService.ObterAcademiasPorFiltroAsync(filtro);

                // Mapear para DTO
                var academiasDto = _mapper.Map<IEnumerable<AcademiaDto>>(resultado.Academias);

                return Ok(new AcademiaFiltroResponseDto
                {
                    Data = academiasDto,
                    Total = resultado.Total,
                    Pagina = filtro.Pagina,
                    TamanhoPagina = filtro.TamanhoPagina,
                    TotalPaginas = (int)Math.Ceiling((double)resultado.Total / filtro.TamanhoPagina)
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (OperacaoAcademiaException ex)
            {
                return StatusCode(500, $"Erro ao buscar academias: {ex.Message}");
            }
        }
      
        /// <summary>
        /// Obtém todas as academias ativas do sistema
        /// </summary>
        /// <returns>Lista de academias</returns>
        /// <response code="200">Retorna a lista de academias</response>
        [HttpGet("todas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodasAcademias()
        {
            var academias = await _academiaService.ObterAcademiasAsync();
            return Ok(academias);
        }

        /// <summary>
        /// Obtém o resumo de todas as academias (ID, Nome, CNPJ, Cidade, Ativo)
        /// </summary>
        /// <returns>Lista de resumos de academias</returns>
        /// <response code="200">Retorna o resumo das academias</response>
        [HttpGet("resumo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoAcademias()
        {
            var resumos = await _academiaService.ObterResumoAcademiasAsync();
            return Ok(resumos);
        }

        /// <summary>
        /// Obtém o resumo de academias com paginação
        /// </summary>
        /// <param name="pagina">Página atual (padrão: 1)</param>
        /// <param name="tamanhoPagina">Quantidade por página (padrão: 10, máximo: 100)</param>
        /// <returns>Lista paginada de resumos e total de registros</returns>
        /// <response code="200">Retorna os resumos paginados e o total</response>
        [HttpGet("resumo-paginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;
            if (tamanhoPagina > 100) tamanhoPagina = 100;

            var (resumos, total) = await _academiaService.ObterResumoAcademiasPaginadoAsync(pagina, tamanhoPagina);

            var resultado = new
            {
                Resumos = resumos,
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                Total = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina)
            };

            return Ok(resultado);
        }

        /// <summary>
        /// Obtém o resumo de academias com base em filtros (nome, CNPJ, município)
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis</param>
        /// <returns>Lista de resumos filtrados</returns>
        /// <response code="200">Retorna os resumos filtrados</response>
        [HttpGet("resumo-filtrado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoFiltrado([FromQuery] FiltroAcademia filtro)
        {
            var resumos = await _academiaService.ObterResumoAcademiasAsync(filtro);
            return Ok(resumos);
        }

        /// <summary>
        /// Obtém o total de academias com base em filtros opcionais
        /// </summary>
        /// <param name="filtro">Filtros (Nome, CNPJ, MunicipioId, FederacaoId, Ativo)</param>
        /// <returns>Número total de academias</returns>
        /// <response code="200">Retorna o total</response>
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTotalAcademias([FromQuery] FiltroAcademia filtro)
        {
            var total = await _academiaService.ObterTotalAcademiasAsync(filtro);
            return Ok(new { Total = total });
        }

        /// <summary>
        /// Obtém o total de academias ativas no sistema
        /// </summary>
        /// <returns>Quantidade de academias ativas</returns>
        /// <response code="200">Retorna o total de ativas</response>
        [HttpGet("total-ativas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTotalAcademiasAtivas()
        {
            var total = await _academiaService.ObterTotalAcademiasAtivasAsync();
            return Ok(new { TotalAtivas = total });
        }

        /// <summary>
        /// Obtém estatísticas gerais das academias (total, ativas, inativas, cidades, federações)
        /// </summary>
        /// <returns>Estatísticas detalhadas</returns>
        /// <response code="200">Retorna as estatísticas</response>
        [HttpGet("estatisticas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterEstatisticasAcademias()
        {
            var estatisticas = await _academiaService.ObterEstatisticasAcademiasAsync();
            return Ok(estatisticas);
        }

        /// <summary>
        /// Verifica se um CNPJ já está em uso por outra academia (para validação em atualizações)
        /// </summary>
        /// <param name="cnpj">CNPJ a ser verificado</param>
        /// <param name="academiaId">ID da academia que está sendo atualizada (excluída da verificação)</param>
        /// <returns>Academia encontrada ou null</returns>
        /// <response code="200">Retorna a academia se CNPJ duplicado</response>
        [HttpGet("validar-cnpj")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidarCnpj([FromQuery] string cnpj, [FromQuery] int academiaId)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return BadRequest("CNPJ é obrigatório.");

            var academia = await _academiaService.ObterPorCnpjUpdateAsync(cnpj, academiaId);

            if (academia == null)
                return Ok(new { Duplicado = false });

            return Ok(new
            {
                Duplicado = true,
                AcademiaId = academia.AcademiaId,
                Nome = academia.Nome,
                CNPJ = academia.Cnpj
            });
        }

        #endregion
    }
}
