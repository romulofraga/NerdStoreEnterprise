using NSE.Core.Mediator;
using NSE.WebApi.Core.Usuario;

namespace NSE.Pagamentos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAspnetUser, AspNetUser>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            return services;
        }
    }
}
