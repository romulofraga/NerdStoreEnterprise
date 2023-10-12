namespace NSE.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
       // public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Dictionary<Guid, int> Itens { get; private set; }

        public PedidoAutorizadoIntegrationEvent(Guid pedidoId, Dictionary<Guid, int> itens)
        {
            //ClienteId = clienteId;
            PedidoId = pedidoId;
            Itens = itens;
        }
    }
}