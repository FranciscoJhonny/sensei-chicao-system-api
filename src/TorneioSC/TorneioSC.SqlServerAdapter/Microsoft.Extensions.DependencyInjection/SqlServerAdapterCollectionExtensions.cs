using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TorneioSC.Domain.Adapters;
using TorneioSC.SqlServerAdapter.Context;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.AcademiaAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.EstadoAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.FederacaoAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.MunicipioAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.PerfilAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.RedeSocialAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.TipoTelefoneAdapters;
using TorneioSC.SqlServerAdapter.SqlServerAdapters.UsuarioAdapters;

namespace TorneioSC.SqlServerAdapter.Microsoft.Extensions.DependencyInjection
{
    public static class SqlServerAdapterCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddSqlServerAdapter(
            this IServiceCollection services,
            SqlServerAdapterConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Registra a configuração como singleton
            services.AddSingleton(configuration);

            // Registra o contexto SQL adaptado
            services.AddScoped<SqlAdapterContext>(provider =>
            {
                var config = provider.GetRequiredService<SqlServerAdapterConfiguration>();
                return SqlAdapterContext.CreateFromConnectionString(
                    config.ConnectionString,
                    config.CommandTimeout);
            });
            // Registrar o DbContext
            //services.AddDbContext<TorneioDbContext>(options =>
            //    options.UseSqlServer(configuration.ConnectionString));

            // Registra os adaptadores
            services.AddScoped<IUsuarioSqlReadAdapter, UsuarioSqlReadAdapter>();
            services.AddScoped<IPerfilSqlReadAdapter, PerfilSqlReadAdapter>();
            //Federação
            services.AddScoped<IFederacaoSqlReadAdapter, FederacaoSqlReadAdapter>();
            services.AddScoped<IFederacaoSqlWriteAdapter, FederacaoSqlWriteAdapter>();

            //Academia
            services.AddScoped<IAcademiaSqlReadAdapter, AcademiaSqlReadAdapter>();
            services.AddScoped<IAcademiaSqlWriteAdapter, AcademiaSqlWriteAdapter>();

            services.AddScoped<IMunicipioSqlReadAdapter, MunicipioSqlReadAdapter>();
            services.AddScoped<IEstadoSqlReadAdapter, EstadoSqlReadAdapter>();
            
            services.AddScoped<ITipoTelefoneSqlReadAdapter, TipoTelefoneSqlReadAdapter>();
            services.AddScoped<IRedeSocialSqlReadAdapter, RedeSocialSqlReadAdapter>();

            return services;
        }
    }
}