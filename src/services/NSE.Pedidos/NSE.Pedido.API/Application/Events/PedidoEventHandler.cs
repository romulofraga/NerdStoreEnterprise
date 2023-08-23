using MediatR;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Pedidos.API.Application.Events
{
    public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
    {
        private readonly IMessageBus _messageBus;

        public PedidoEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        public async Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
        {
            await _messageBus.PublishAsync(new PedidoRealizadoIntegrationEvent(message.ClienteId));
        }
    }
}
