using System.Net;
using System.Text.Json;

namespace TorneioSC.WebApi.Middlewares
{
    public class UnauthorizedResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Intercepta a resposta
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // Processa apenas respostas 401
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                // Remove cabeçalhos padrão
                context.Response.Headers.Remove("WWW-Authenticate");

                // Define o tipo de conteúdo
                context.Response.ContentType = "application/json";

                // Cria a resposta personalizada
                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = GetErrorMessage(context),
                    Timestamp = DateTime.UtcNow
                };

                // Serializa e escreve a resposta
                var jsonResponse = JsonSerializer.Serialize(response);
                context.Response.ContentLength = jsonResponse.Length;

                // Prepara o stream de resposta
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // Para outras respostas, mantém o fluxo original
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private string GetErrorMessage(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
                return "Token de autenticação não fornecido.";

            return "Token inválido ou expirado.";
        }
    }
}