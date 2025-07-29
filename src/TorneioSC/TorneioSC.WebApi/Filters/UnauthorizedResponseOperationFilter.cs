using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace TorneioSC.WebApi.Filters
{
    public class UnauthorizedResponseOperationFilter : IOperationFilter
    {
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