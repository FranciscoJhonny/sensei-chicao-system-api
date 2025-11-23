using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;
using TorneioSC.WebApi.Dtos.UsuarioDtos;
using TorneioSC.WebApi.Services;

namespace TorneioSC.WebApi.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de usuários
    /// </summary>
    [Route("api/usuario/")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        /// <summary>
        /// Construtor do controller de usuários
        /// </summary>
        /// <param name="mapper">Mapper para conversão de DTOs</param>
        /// <param name="usuarioService">Serviço de usuários</param>
        /// <param name="loggerFactory">Factory para criação de loggers</param>
        public UsuarioController(IMapper mapper,
           IUsuarioService usuarioService,
           ILoggerFactory loggerFactory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _logger = loggerFactory?.CreateLogger<UsuarioController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #region 🔽 Métodos de Leitura

        /// <summary>
        /// Obtém a lista de todos os usuários cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        /// <response code="200">Retorna a lista de usuários</response>
        /// <response code="404">Nenhum usuário encontrado</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        //[Authorize(Roles = "Adminstrador")]
        [HttpGet("get-lista-usuario")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Obtém um usuário específico pelo seu ID
        /// </summary>
        /// <param name="id">ID do usuário a ser obtido</param>
        /// <returns>Usuário encontrado</returns>
        /// <response code="200">Retorna o usuário solicitado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}", Name = "ObterUsuarioPorId")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterUsuarioPorIdAsync(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);

            var response = _mapper.Map<UsuarioDto>(usuario);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        /// <summary>
        /// Verifica se um usuário existe pelo email
        /// </summary>
        /// <param name="email">Email do usuário a ser verificado</param>
        /// <returns>True se o usuário existe, False caso contrário</returns>
        /// <response code="200">Retorna o resultado da verificação</response>
        /// <response code="400">Email inválido</response>
        /// <response code="500">Erro interno do servidor</response>
        //[Authorize(Roles = "Adminstrador")]
        [HttpGet("get-verifica-usuario")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
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

        #endregion

        #region 🔽 Métodos de Escrita

        /// <summary>
        /// Realiza autenticação do usuário
        /// </summary>
        /// <param name="loginUsuarioDto">Dados de login do usuário</param>
        /// <returns>Token de acesso JWT</returns>
        /// <response code="200">Autenticação bem-sucedida, retorna token</response>
        /// <response code="400">Credenciais inválidas</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("auth")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="usuarioDto">Dados do usuário a ser criado</param>
        /// <returns>Usuário criado com ID atribuído</returns>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="403">Sem permissão</response>
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

        /// <summary>
        /// Solicita redefinição de senha
        /// </summary>
        /// <param name="dto">DTO com email para redefinição</param>
        /// <returns>Token de redefinição</returns>
        /// <response code="200">Solicitação processada com sucesso</response>
        /// <response code="400">Usuário não encontrado</response>
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

        /// <summary>
        /// Redefine a senha do usuário usando token de recuperação
        /// </summary>
        /// <param name="dto">DTO com token e nova senha</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Senha redefinida com sucesso</response>
        /// <response code="400">Token inválido ou expirado</response>
        /// <response code="500">Erro interno do servidor</response>
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

        /// <summary>
        /// Atualiza os dados de um usuário existente
        /// </summary>
        /// <param name="usuarioDto">Dados atualizados do usuário</param>
        /// <returns>Usuário atualizado</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        //[Authorize(Roles = "Adminstrador")]
        [HttpPut("put-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUsuario([FromBody] UsuarioPutDto usuarioDto)
        {
            if (usuarioDto is null)
                throw new ArgumentNullException(nameof(usuarioDto));

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

        /// <summary>
        /// Exclui um usuário pelo ID
        /// </summary>
        /// <param name="usuarioId">ID do usuário a ser excluído</param>
        /// <returns>Resultado da exclusão</returns>
        /// <response code="200">Usuário excluído com sucesso</response>
        /// <response code="400">ID inválido</response>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        //[Authorize(Roles = "Adminstrador")]
        [HttpPut("delete-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        #endregion

        #region 🔽 Validações

        // Como a validação é feita via Data Annotations e ModelState,
        // esta região ficará vazia até que validações customizadas sejam necessárias
        // Exemplo futuro:
        // private bool ValidarEmailUnico(string email) { ... }
        // private bool ValidarForcaSenha(string senha) { ... }

        #endregion
    }
}