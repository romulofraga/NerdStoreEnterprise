﻿namespace NSE.Core.Messages.Integration
{
    public class PedidoBaixadoEstoqueIntegrationEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        //public Guid ClienteId { get; private set; }

        public PedidoBaixadoEstoqueIntegrationEvent(Guid pedidoId)
        {
            PedidoId = pedidoId;
            // ClienteId = clienteId;
        }
    }
}