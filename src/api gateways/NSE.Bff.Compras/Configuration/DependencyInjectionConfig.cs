using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services;
using NSE.WebApi.Core.Extensions;
using NSE.WebApi.Core.Usuario;

namespace NSE.Bff.Compras.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspnetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegateHandler>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(PollyExtensions.CircuitBreakerConfig);

            services.AddHttpClient<ICarrinhoService, CarrinhoService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
               .AddPolicyHandler(PollyExtensions.EsperarTentar())
               .AddTransientHttpErrorPolicy(PollyExtensions.CircuitBreakerConfig);

            services.AddHttpClient<IPedidoService, PedidoService>()
              .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
              .AddPolicyHandler(PollyExtensions.EsperarTentar())
              .AddTransientHttpErrorPolicy(PollyExtensions.CircuitBreakerConfig);


            //services.AddHttpClient<ICarrinhoService, CarrinhoService>()
            //   .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
            //   .AddPolicyHandler(PollyExtensions.EsperarTentar())
            //   .AddTransientHttpErrorPolicy(
            //   p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            return services;
        }
    }
}
