using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.WebApi.Core.Usuario;

namespace NSE.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspnetUser, AspnetUser>();
        services.AddDbContext<CarrinhoContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        //services.AddScoped<IMediatorHandler, MediatorHandler>();
        //services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

        //services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

        //services.AddScoped<IClienteRepository, ClienteRepository>();
        //services.AddScoped<ClientesContext>();

        return services;
    }
}