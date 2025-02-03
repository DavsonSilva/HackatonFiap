using Hackaton.Domain.Repositories;
using Hackaton.Domain.Services;
using Hackaton.Infra.Data.Context;
using Hackaton.Infra.Data.Repositories;
using Hackaton.Infra.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackaton.Infra.Ioc
{
    public static class DependencyResolver
    {
        public static IServiceCollection ConfigureApplicationContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddServices(configuration);
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureServices(services);
            ConfigureRepositories(services);

            services.AddDbContext<FiapDbContext>(optionsBuilder =>
                    optionsBuilder.UseNpgsql(configuration.GetSection("PostGreeDbSettings").GetValue<string>("ConnectionString")));
            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAgendaService, AgendaService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IConsultaService, ConsultaService>();
            services.AddScoped<IMedicoService, MedicoService>();
            services.AddScoped<INotificacaoService, NotificacaoService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<ISendGridService, SendGridService>();
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IAgendaRepository, AgendaRepository>();
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }
    }
}
