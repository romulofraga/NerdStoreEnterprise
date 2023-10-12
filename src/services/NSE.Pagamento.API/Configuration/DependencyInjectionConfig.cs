using NSE.Pagamentos.API.Data.Repository;
using NSE.Pagamentos.API.Facade;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.API.Services;
using NSE.WebApi.Core.Usuario;

namespace NSE.Pagamentos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspnetUser, AspNetUser>();

        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
    }
}