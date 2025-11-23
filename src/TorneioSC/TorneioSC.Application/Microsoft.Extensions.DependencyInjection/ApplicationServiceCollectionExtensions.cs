using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TorneioSC.Application.Services;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Métodos de extensão para configuração de serviços da camada de Application
    /// </summary>
    public static class ApplicationServiceCollectionExtensions
    {
        /// <summary>
        /// Adiciona todos os serviços da camada de Application ao container de DI
        /// </summary>
        /// <param name="services">Coleção de serviços do ASP.NET Core</param>
        /// <returns>Coleção de serviços para method chaining</returns>
        /// <exception cref="ArgumentNullException">Lançada quando services é nulo</exception>
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddApplicationService(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Registra todos os serviços da camada de Application
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IFederacaoService, FederacaoService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IAcademiaService, AcademiaService>();
            services.AddScoped<ITipoTelefoneService, TipoTelefoneService>();
            services.AddScoped<IRedeSocialService, RedeSocialService>();
            services.AddScoped<IEventoService, EventoService>();

            return services;
        }
    }
}