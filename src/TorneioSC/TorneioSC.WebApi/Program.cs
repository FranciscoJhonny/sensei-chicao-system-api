using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using TorneioSC.Application.Microsoft.Extensions.DependencyInjection;
using TorneioSC.SqlServerAdapter.Context;
using TorneioSC.SqlServerAdapter.Microsoft.Extensions.DependencyInjection;
using TorneioSC.WebApi;
using TorneioSC.WebApi.Filters;
using TorneioSC.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configuração
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(Key.Secret);
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();
// Configuração do SQL Server Adapter
var sqlAdapterConfig = new SqlServerAdapterConfiguration
{
    ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
    CommandTimeout = 30,
    EnableTransaction = true
};

// Registrar serviços
builder.Services.AddSqlServerAdapter(sqlAdapterConfig);
builder.Services.AddApplicationService();
builder.Services.AddAutoMapper(typeof(WebApiMapperProfile).Assembly);

// Configuração do Swagger com JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TorneioSC API", Version = "v1" });

    // Configuração para suportar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    c.OperationFilter<UnauthorizedResponseOperationFilter>();
});
// Adiciona o filtro global de exceções
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});
builder.Services.AddLogging(loggingBuilder => {
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = 401,
            Message = "Acesso não autorizado",
            Timestamp = DateTime.UtcNow
        }));
    }
});

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANTE: UseAuthentication antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();
// Middleware para customizar respostas 401
app.UseMiddleware<UnauthorizedResponseMiddleware>();
app.MapControllers();

app.Run();