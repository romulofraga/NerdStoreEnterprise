using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Carrinho.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public CarrinhoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", ApagarCarrinho);
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

            var carrinho = await context.CarrinhoClientes.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

            if (carrinho != null) await context.SaveChangesAsync();
        }
    }
}
