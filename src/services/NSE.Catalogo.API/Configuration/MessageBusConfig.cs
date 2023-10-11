using NSE.Catalogo.API.Services;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Catalogo.API.Configurarion
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQeueConnection("MessageBus"));
            services.AddHostedService<CatalogoIntegrationHandler>();
        }
    }
}
