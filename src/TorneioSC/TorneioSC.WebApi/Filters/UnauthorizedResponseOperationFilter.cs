using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TorneioSC.WebApi.Filters
{
    /// <summary>
    /// Filtro do Swagger para adicionar resposta padronizada 401 (Unauthorized) em todas as operações
    /// </summary>
    public class UnauthorizedResponseOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Aplica o filtro para adicionar a resposta 401 em todas as operações da API
        /// </summary>
        /// <param name="operation">Operação OpenAPI sendo processada</param>
        /// <param name="context">Contexto do filtro de operação</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "Unauthorized",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Example = new OpenApiObject
                            {
                                ["StatusCode"] = new OpenApiInteger(401),
                                ["Message"] = new OpenApiString("Token inválido ou ausente"),
                                ["Timestamp"] = new OpenApiString(Convert.ToString(DateTime.UtcNow))
                            }
                        }
                    }
                }
            });
        }
    }
}