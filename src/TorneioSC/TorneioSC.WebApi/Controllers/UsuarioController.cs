using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.UsuarioDtos;
using TorneioSC.WebApi.Services;

namespace TorneioSC.WebApi.Controllers
{
    [Route("api/usuario/")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UsuarioController(IMapper mapper,
           IUsuarioService UsuarioService,
           ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usuarioService = UsuarioService ?? throw new ArgumentNullException(nameof(UsuarioService));
            _logger = loggerFactory?.CreateLogger<UsuarioController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        //[Authorize(Roles = "Adminstrador")]
        [HttpGet("get-lista-usuario")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarioesAsync()
        {
            var usuario = await _usuarioService.ObterUsuarioAsync();

            var response = _mapper.Map<IEnumerable<UsuarioDto>>(usuario);

            if (response == null)
                return NotFound();

            return Ok(response);

        }

        [HttpGet("{id}", Name = "ObterUsuarioPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);

            var response = _mapper.Map<UsuarioDto>(usuario);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        //[Authorize(Roles = "Adminstrador")]
        [HttpGet("get-verifica-usuario")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)] // Corrigido para bool
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> GetVerificaUsuarioAsync(string email) // Usando ActionResult para melhor documentação
        {
            try
            {
                var existeUsuario = await _usuarioService.VerificaUsuarioAsync(email);
                return existeUsuario > 0; // Retorna true se > 0, false caso contrário
            }
            catch (System.Exception ex) // System não é necessário
            {
                _logger.LogError(ex, "Erro ao verificar usuário com email {Email}", email);
                throw; // Simplesmente "throw" mantém a stack trace original
            }
        }

        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Auth([FromBody] LoginUsuarioDto loginUsuarioDto)
        {
            if (loginUsuarioDto.Usuario == null)
                throw new ArgumentNullException(nameof(loginUsuarioDto.Usuario));
            if (loginUsuarioDto.Senha == null)
                throw new ArgumentNullException(nameof(loginUsuarioDto.Senha));

            var dbusuario = await _usuarioService.LoginUsuario(loginUsuarioDto.Usuario, loginUsuarioDto.Senha);

            if (dbusuario != null)
            {
                var access_token = TokenService.GenerateToken(dbusuario);
                return Ok(access_token);
            }

            return BadRequest("Credenciais inválidas.");
        }

        //[Authorize]
        [HttpPost("post-usuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostUsuario([FromBody] UsuarioPostDto usuarioDto)
        {
            var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            var novoUsuario = await _usuarioService.PostUsuario(usuario, usuarioLogadoId);

            return CreatedAtRoute(
                "ObterUsuarioPorId",
                new { id = novoUsuario.UsuarioId },
                new
                {
                    novoUsuario.UsuarioId,
                    novoUsuario.Nome,
                    novoUsuario.Email,
                    novoUsuario.PerfilId
                });
        }

        [HttpPost("solicitar-redefinicao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SolicitarRedefinicao([FromBody] SolicitarRedefinicaoSenhaDto dto)
        {
            var usuario = await _usuarioService.ObterPorEmailAsync(dto.Email);
            if (usuario == null)
                return BadRequest("Usuário não encontrado.");

            var token = Guid.NewGuid().ToString().ToUpper();
            var validade = DateTime.UtcNow.AddHours(1); // Token válido por 1 hora

            await _usuarioService.SalvarTokenRedefinicaoAsync(usuario.UsuarioId, token, validade);

            // Por enquanto retornamos o token no corpo da resposta (em produção seria enviado por e-mail)
            return Ok(new { token });
        }

        [HttpPost("redefinir-senha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDto dto)
        {
            var usuario = await _usuarioService.ObterPorTokenRecuperacaoAsync(dto.Token);
            if (usuario == null || usuario.DataRecuperacaoSenha < DateTime.UtcNow)
                return BadRequest("Token inválido ou expirado.");

            await _usuarioService.AtualizarSenhaAsync(usuario.UsuarioId, dto.NovaSenha);

            await _usuarioService.LimparTokenRecuperacaoAsync(usuario.UsuarioId);

            return Ok("Senha redefinida com sucesso.");
        }

        //[Authorize(Roles = "Adminstrador")]
        [HttpPut("put-usuario")]
        [ProducesResponseType(typeof(UsuarioPutDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUsuario([FromBody] UsuarioPutDto usuarioDto)
        {
            if (usuarioDto is null)
                throw new ArgumentNullException(nameof(usuarioDto));
            // Obter ID do usuário autenticado do token JWT
            var usuarioLogadoId = int.Parse(User.FindFirst("usuarioId")?.Value ?? "0");

            var usuario = _mapper.Map<Usuario>(usuarioDto);

            var novoUsuario = await _usuarioService.UpdateUsuario(usuario, usuarioLogadoId);

            return CreatedAtRoute(
               "ObterUsuarioPorId",
               new { id = novoUsuario.UsuarioId },
               new
               {
                   novoUsuario.UsuarioId,
                   novoUsuario.Nome,
                   novoUsuario.Email,
                   novoUsuario.PerfilId
               });
        }

       

        //[Authorize(Roles = "Adminstrador")]
        [HttpPut("delete-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarUsuario(int usuarioId)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);

            var response = _mapper.Map<UsuarioDto>(usuario);

            if (response == null)
                return NotFound();

            var result = await _usuarioService.DeleteUsuarioPorIdAsync(usuarioId);

            return Ok(result);

        }

       

    }
}
