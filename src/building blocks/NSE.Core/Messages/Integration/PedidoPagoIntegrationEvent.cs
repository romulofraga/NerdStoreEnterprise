﻿namespace NSE.Core.Messages.Integration;

public class PedidoPagoIntegrationEvent : IntegrationEvent
{
    public PedidoPagoIntegrationEvent(Guid pedidoId)
    {
        //  ClienteId = clienteId;
        PedidoId = pedidoId;
    }

    // public Guid ClienteId { get; private set; }
    public Guid PedidoId { get; private set; }
}