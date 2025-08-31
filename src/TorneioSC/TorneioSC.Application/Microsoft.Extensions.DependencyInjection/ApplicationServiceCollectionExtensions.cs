using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TorneioSC.Application.Services;
using TorneioSC.Domain.Services;

namespace TorneioSC.Application.Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddApplicationService(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IFederacaoService, FederacaoService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IAcademiaService, AcademiaService>();
            services.AddScoped<ITipoTelefoneService, TipoTelefoneService>();



            return services;
        }
    }
}
