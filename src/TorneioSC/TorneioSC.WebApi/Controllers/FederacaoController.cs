using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.FederacaoDtos;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/federacao/")]
    [ApiController]
    public class FederacaoController : ControllerBase
    {
        private readonly IFederacaoService _federacaoService;
        private readonly IMapper _mapper;

        public FederacaoController(IMapper mapper,
           IFederacaoService federacaoService,
           ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _federacaoService = federacaoService ?? throw new ArgumentNullException(nameof(federacaoService));
        }

        
        //[Authorize]
        [HttpGet("get-lista-federacao")]
        [ProducesResponseType(typeof(FederacaoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFederacoesAsync()
        {
            var federacoes = await _federacaoService.ObterFederacaoAsync();

            var response = _mapper.Map<IEnumerable<FederacaoDto>>(federacoes);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        //[Authorize]
        [HttpGet("{id}", Name = "ObterFederacaoPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterFederacaoPorIdAsync(int id)
        {
            var federacao = await _federacaoService.ObterFederacaoPorIdAsync(id);
            if (federacao == null)
                return NotFound();

            return Ok(_mapper.Map<FederacaoDto>(federacao));
        }

        //[Authorize]
        [HttpGet("get-por-cnpj/{cnpj}")]
        [ProducesResponseType(typeof(FederacaoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorCnpjAsync(string cnpj)
        {
            var federacao = await _federacaoService.ObterPorCnpjAsync(cnpj);
            if (federacao == null)
                return NotFound();

            return Ok(_mapper.Map<FederacaoDto>(federacao));
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("post-federacao")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostFederacao([FromBody] FederacaoPostDto federacaoDto)
        {
            var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var federacao = _mapper.Map<Federacao>(federacaoDto);
            var novaFederacaoId = await _federacaoService.PostFederacaoAsync(federacao, usuarioLogadoId);

            var federacaoCriada = await _federacaoService.ObterFederacaoPorIdAsync(novaFederacaoId);

            return CreatedAtRoute(
                "ObterFederacaoPorId",
                new { id = novaFederacaoId },
                _mapper.Map<FederacaoDto>(federacaoCriada));
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("put-federacao")]
        [ProducesResponseType(typeof(FederacaoPutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutFederacao([FromBody] FederacaoPutDto federacaoDto)
        {
            if (federacaoDto is null)
                throw new ArgumentNullException(nameof(federacaoDto));

            var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

            var federacao = _mapper.Map<Federacao>(federacaoDto);
            var federacaoAtualizadaId = await _federacaoService.UpdateFederacaoAsync(federacao, usuarioLogadoId);

            var federacaoAtualizada = await _federacaoService.ObterFederacaoPorIdAsync(federacaoAtualizadaId);

            return Ok(_mapper.Map<FederacaoDto>(federacaoAtualizada));
        }

        ////[Authorize(Roles = "Administrador")]
        //[HttpPut("vincular-endereco")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> VincularEndereco([FromBody] VincularEnderecoDto dto)
        //{
        //    var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

        //    var result = await _federacaoService.VincularEnderecoAsync(dto.FederacaoId, dto.EnderecoId, usuarioLogadoId);

        //    if (result <= 0)
        //        return BadRequest("Não foi possível vincular o endereço");

        //    return Ok(new { success = true, message = "Endereço vinculado com sucesso" });
        //}

        ////[Authorize(Roles = "Administrador")]
        //[HttpPut("vincular-telefone")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> VincularTelefone([FromBody] VincularTelefoneDto dto)
        //{
        //    var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

        //    var result = await _federacaoService.VincularTelefoneAsync(dto.FederacaoId, dto.TelefoneId, usuarioLogadoId);

        //    if (result <= 0)
        //        return BadRequest("Não foi possível vincular o telefone");

        //    return Ok(new { success = true, message = "Telefone vinculado com sucesso" });
        //}

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("delete-federacao/{federacaoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarFederacao(int federacaoId)
        {
            var federacao = await _federacaoService.ObterFederacaoPorIdAsync(federacaoId);

            if (federacao == null)
                return NotFound();

            var result = await _federacaoService.DeleteFederacaoPorIdAsync(federacaoId);

            return Ok(new { success = result, message = result ? "Federação deletada com sucesso" : "Falha ao deletar federação" });
        }
    }
}