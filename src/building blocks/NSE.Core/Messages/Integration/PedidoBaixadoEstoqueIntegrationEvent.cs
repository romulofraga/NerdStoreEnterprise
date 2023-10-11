namespace NSE.Core.Messages.Integration
{
    public class PedidoBaixadoEstoqueIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId;
        public Guid PedidoId;

        public PedidoBaixadoEstoqueIntegrationEvent(Guid clienteId, Guid pedidoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}