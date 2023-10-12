﻿using NSE.Core.Messages.Integration;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Pagamentos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQeueConnection("MessageBus")).AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}
