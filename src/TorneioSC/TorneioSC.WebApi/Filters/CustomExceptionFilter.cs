using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;
using TorneioSC.Exception.ExceptionBase.ExceptionPerfil;
using TorneioSC.Exception.ExceptionBase.ExceptionUsuario;

namespace TorneioSC.WebApi.Filters
{
    /// <summary>
    /// Filtro personalizado para tratamento global de exceções na aplicação
    /// </summary>
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        /// <summary>
        /// Construtor do filtro de exceções
        /// </summary>
        /// <param name="logger">Logger para registro de eventos</param>
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método executado quando uma exceção ocorre na aplicação
        /// </summary>
        /// <param name="context">Contexto da exceção contendo informações sobre o erro</param>
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.LogError(exception, "Erro não tratado na aplicação");

            // Verifica primeiro se é uma exceção de autorização
            if (context.Exception is Microsoft.AspNetCore.Authentication.AuthenticationFailureException ||
                context.Exception is UnauthorizedAccessException ||
                (context.Exception is System.Security.SecurityException &&
                 context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized))
            {
                CreateResponse(context, HttpStatusCode.Unauthorized, "Acesso não autorizado. Token inválido ou ausente.");
                context.ExceptionHandled = true;
                return;
            }

            switch (exception)
            {
                case ValidacaoUsuarioException validacaoEx:
                    CreateResponse(context, HttpStatusCode.BadRequest, "Erros de validação", validacaoEx.ErrosValidacao);
                    break;

                case ValidacaoFederacaoException validacaoFedEx:
                    CreateResponse(context, HttpStatusCode.BadRequest, "Erros de validação da federação", validacaoFedEx.ErrosValidacao);
                    break;

                case CredenciaisInvalidasException:
                    CreateResponse(context, HttpStatusCode.Unauthorized, exception.Message);
                    break;

                case UsuarioInativoException:
                case FederacaoInativaException:
                    CreateResponse(context, HttpStatusCode.Forbidden, exception.Message);
                    break;

                case PerfilNaoEncontradoException:
                case FederacaoNaoEncontradaException:
                    CreateResponse(context, HttpStatusCode.NotFound, exception.Message);
                    break;

                // Exceções de Usuário
                case UsuarioNaoEncontradoException:
                    CreateResponse(context, HttpStatusCode.NotFound, exception.Message);
                    break;

                case EmailEmUsoException:
                case CnpjEmUsoException:
                    CreateResponse(context, HttpStatusCode.Conflict, exception.Message);
                    break;

                case OperacaoUsuarioException:
                case OperacaoFederacaoException:
                case AtualizacaoFederacaoException:
                case ExclusaoFederacaoException:
                case VinculoEnderecoException:
                case VinculoTelefoneException:
                    CreateResponse(context, HttpStatusCode.BadRequest, exception.Message);
                    break;

                case CnpjInvalidoException:
                    CreateResponse(context, HttpStatusCode.UnprocessableEntity, exception.Message);
                    break;

                // Exceções de sistema
                case UnauthorizedAccessException:
                    CreateResponse(context, HttpStatusCode.Unauthorized, "Acesso não autorizado");
                    break;

                case ArgumentException:
                    CreateResponse(context, HttpStatusCode.BadRequest, exception.Message);
                    break;

                default:
                    // Erro não tratado especificamente
                    CreateResponse(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor");
                    break;
            }

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Cria a resposta padronizada para exceções
        /// </summary>
        /// <param name="context">Contexto da exceção</param>
        /// <param name="statusCode">Código HTTP de status</param>
        /// <param name="message">Mensagem de erro</param>
        /// <param name="errors">Lista de erros de validação (opcional)</param>
        private void CreateResponse(ExceptionContext context, HttpStatusCode statusCode, string message, List<string> errors = null)
        {
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Errors = errors,  // Adiciona a lista de erros quando existir
                Timestamp = DateTime.UtcNow
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}