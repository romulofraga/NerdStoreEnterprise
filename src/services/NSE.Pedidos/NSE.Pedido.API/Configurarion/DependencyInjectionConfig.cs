using Microsoft.EntityFrameworkCore;
using NSE.Core.Mediator;
using NSE.Pedidos.Infra.Data;

namespace NSE.Pedidos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PedidosContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            //services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            //services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            //services.AddScoped<IClienteRepository, ClienteRepository>();
            //services.AddScoped<ClientesContext>();

            return services;
        }
    }
}
