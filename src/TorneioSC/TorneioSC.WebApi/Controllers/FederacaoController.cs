using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Models.Filtros;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using System.ComponentModel.DataAnnotations;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/federacao")]
    [ApiController]
    public class FederacaoController : ControllerBase
    {
        private readonly IFederacaoService _federacaoService;
        private readonly IMapper _mapper;

        public FederacaoController(IMapper mapper, IFederacaoService federacaoService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _federacaoService = federacaoService ?? throw new ArgumentNullException(nameof(federacaoService));
        }

        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Cria uma nova federação
        /// </summary>
        /// <param name="federacaoDto">Dados da federação</param>
        /// <returns>Federação criada</returns>
        /// <response code="201">Federação criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost("post-federacao")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostFederacao([FromBody] FederacaoPostDto federacaoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federacao = _mapper.Map<Federacao>(federacaoDto);
            var novaFederacaoId = await _federacaoService.PostFederacaoAsync(federacao);

            var federacaoCriada = await _federacaoService.ObterFederacaoPorIdAsync(novaFederacaoId);

            return CreatedAtRoute(
                "ObterFederacaoPorId",
                new { id = novaFederacaoId },
                _mapper.Map<FederacaoDto>(federacaoCriada));
        }

        /// <summary>
        /// Atualiza uma federação existente
        /// </summary>
        /// <param name="federacaoDto">Dados atualizados</param>
        /// <returns>Federação atualizada</returns>
        /// <response code="200">Federação atualizada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPut("put-federacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutFederacao([FromBody] FederacaoPutDto federacaoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federacao = _mapper.Map<Federacao>(federacaoDto);
            var linhasAfetadas = await _federacaoService.PutFederacaoAsync(federacao);

            if (linhasAfetadas == 0)
                return NotFound(new { message = "Federação não encontrada para atualização." });

            var federacaoAtualizada = await _federacaoService.ObterFederacaoPorIdAsync(federacao.FederacaoId);
            return Ok(_mapper.Map<FederacaoDto>(federacaoAtualizada));
        }

        /// <summary>
        /// Inativa uma federação (exclusão lógica)
        /// </summary>
        /// <param name="dto">ID da federação e usuário que está inativando</param>
        /// <returns>Mensagem de sucesso ou falha</returns>
        /// <response code="200">Inativação realizada</response>
        /// <response code="404">Federação não encontrada</response>
        [HttpPut("inativar-federacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InativarFederacao([FromBody] FederacaoInativacaoDto dto)
        {
            if (dto == null)
                return BadRequest("Dados de inativação são obrigatórios.");

            var federacao = await _federacaoService.ObterFederacaoPorIdAsync(dto.FederacaoId);
            if (federacao == null)
                return NotFound();

            var sucesso = await _federacaoService.InativarFederacaoPorIdAsync(dto.FederacaoId, dto.UsuarioOperacaoId);

            return Ok(new
            {
                success = sucesso,
                message = sucesso ? "Federação inativada com sucesso." : "Falha ao inativar federação."
            });
        }

        #endregion

        #region 🔽 Métodos de leitura

        /// <summary>
        /// Obtém uma federação pelo CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ da federação (com ou sem máscara)</param>
        /// <returns>Federação encontrada</returns>
        /// <response code="200">Retorna a federação</response>
        /// <response code="404">Não encontrada</response>
        [HttpGet("get-por-cnpj")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorCnpjAsync([FromQuery] string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return BadRequest("CNPJ é obrigatório.");

            var federacao = await _federacaoService.ObterPorCnpjAsync(cnpj);
            if (federacao == null)
                return NotFound();

            return Ok(_mapper.Map<FederacaoDto>(federacao));
        }

        /// <summary>
        /// Obtém uma federação pelo ID
        /// </summary>
        /// <param name="id">ID da federação</param>
        /// <returns>Federação encontrada</returns>
        /// <response code="200">Retorna a federação</response>
        /// <response code="404">Não encontrada</response>
        [HttpGet("get-por-id/{id}", Name = "ObterFederacaoPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var federacao = await _federacaoService.ObterFederacaoPorIdAsync(id);
            if (federacao == null)
                return NotFound();

            return Ok(_mapper.Map<FederacaoDto>(federacao));
        }

        [HttpGet("filtro")]
        [ProducesResponseType(typeof(FederacaoFiltroResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterFederacoesPorFiltro([FromQuery] FiltroFederacao filtro)
        {
            var resultado = await _federacaoService.ObterFederacoesPorFiltroAsync(filtro);

            var data = _mapper.Map<IEnumerable<FederacaoDto>>(resultado.Federacoes);

            return Ok(new
            {
                Data = data,
                Total = resultado.Total,
                Pagina = filtro.Pagina,
                TamanhoPagina = filtro.TamanhoPagina,
                TotalPaginas = (int)Math.Ceiling((double)resultado.Total / filtro.TamanhoPagina)
            });
        }

        /// <summary>
        /// Obtém todas as federações ativas
        /// </summary>
        /// <returns>Lista de federações</returns>
        /// <response code="200">Retorna todas as federações</response>
        [HttpGet("todas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodasFederacoes()
        {
            var federacoes = await _federacaoService.ObterFederacaoAsync();
            return Ok(_mapper.Map<IEnumerable<FederacaoDto>>(federacoes));
        }

        /// <summary>
        /// Verifica se um CNPJ já está em uso por outra federação (para atualizações)
        /// </summary>
        /// <param name="cnpj">CNPJ a verificar</param>
        /// <param name="federacaoId">ID da federação sendo editada (excluída da verificação)</param>
        /// <returns>Indica se o CNPJ está duplicado</returns>
        /// <response code="200">Retorna status de duplicação</response>
        [HttpGet("validar-cnpj")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidarCnpj([FromQuery] string cnpj, [FromQuery] int federacaoId)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return BadRequest("CNPJ é obrigatório.");

            var federacao = await _federacaoService.ObterPorCnpjUpdateAsync(cnpj, federacaoId);

            if (federacao == null)
                return Ok(new { Duplicado = false });

            return Ok(new
            {
                Duplicado = true,
                FederacaoId = federacao.FederacaoId,
                Nome = federacao.Nome,
                CNPJ = federacao.Cnpj
            });
        }

        /// <summary>
        /// Obtém resumo de todas as federações (ID, Nome, CNPJ, Cidade, Ativo)
        /// </summary>
        /// <returns>Lista de resumos</returns>
        /// <response code="200">Retorna o resumo</response>
        [HttpGet("resumo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoFederacoes()
        {
            var resumos = await _federacaoService.ObterResumoFederacoesAsync();
            return Ok(resumos);
        }

        /// <summary>
        /// Obtém resumo de federações com paginação
        /// </summary>
        /// <param name="pagina">Página (padrão: 1)</param>
        /// <param name="tamanhoPagina">Itens por página (padrão: 10, máx: 100)</param>
        /// <returns>Resumos paginados e total</returns>
        /// <response code="200">Retorna os dados paginados</response>
        [HttpGet("resumo-paginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1 || tamanhoPagina > 100) tamanhoPagina = 10;

            var (resumos, total) = await _federacaoService.ObterResumoFederacoesPaginadoAsync(pagina, tamanhoPagina);

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
        /// Obtém resumo de federações com filtros (nome, CNPJ, município)
        /// </summary>
        /// <param name="filtro">Filtros aplicáveis</param>
        /// <returns>Resumos filtrados</returns>
        /// <response code="200">Retorna os resumos filtrados</response>
        [HttpGet("resumo-filtrado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterResumoFiltrado([FromQuery] FiltroFederacao filtro)
        {
            var resumos = await _federacaoService.ObterResumoFederacoesAsync(filtro);
            return Ok(resumos);
        }

        /// <summary>
        /// Obtém o total de federações com base em filtros
        /// </summary>
        /// <param name="filtro">Filtros (Nome, CNPJ, MunicipioId, Ativo)</param>
        /// <returns>Total de registros</returns>
        /// <response code="200">Retorna o total</response>
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTotalFederacoes([FromQuery] FiltroFederacao filtro)
        {
            var total = await _federacaoService.ObterTotalFederacoesAsync(filtro);
            return Ok(new { Total = total });
        }

        /// <summary>
        /// Obtém estatísticas gerais das federações (total, ativas, inativas, estados, cidades)
        /// </summary>
        /// <returns>Estatísticas detalhadas</returns>
        /// <response code="200">Retorna as estatísticas</response>
        [HttpGet("estatisticas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterEstatisticasFederacoes()
        {
            var estatisticas = await _federacaoService.ObterEstatisticasFederacoesAsync();
            return Ok(estatisticas);
        }

        

        #endregion
    }
}