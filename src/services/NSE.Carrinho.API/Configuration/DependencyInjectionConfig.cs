using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.WebApi.Core.Usuario;

namespace NSE.Carrinho.API.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CarrinhoContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspnetUser, AspNetUser>();
        return services;
    }
}