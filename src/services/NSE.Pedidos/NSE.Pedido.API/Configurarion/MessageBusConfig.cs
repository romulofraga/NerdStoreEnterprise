using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pedidos.API.Services;

namespace NSE.Pedidos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQeueConnection("MessageBus")).AddHostedService<PedidoOrquestradorIntegrationHandler>();
        }
    }
}
