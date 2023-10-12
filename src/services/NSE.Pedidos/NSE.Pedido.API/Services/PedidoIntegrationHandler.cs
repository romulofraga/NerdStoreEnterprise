using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Services;

public class PedidoIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly ILogger<PedidoIntegrationHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public PedidoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus,
        ILogger<PedidoIntegrationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();
        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado",
            async request => await CancelarPedido(request));

        _bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago",
            async request => await FinalizarPedido(request));

        #region Gambiarra

        // _bus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque",
        //     async request => await FinalizarPedidoGambiarra(request));

        #endregion
    }

    private async Task FinalizarPedidoGambiarra(PedidoBaixadoEstoqueIntegrationEvent message)
    {
        #region GAMBIARRAFINALIZARPEDIDO

        if (message.PedidoId == Guid.Empty)
        {
            _logger.LogCritical("CHEGOU NO FINALIZAR PEDIDO GAMBIARRA SEM GUID - {GUID}", message.PedidoId);
            return;
        }

        #endregion

        using var scope = _serviceProvider.CreateScope();

        var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepository.ObterPorId(message.PedidoId);

        pedido.FinalizarPedido();

        pedidoRepository.Atualizar(pedido);

        if (!await pedidoRepository.UnityOfWork.Commit())
            throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");

        _logger.LogCritical("Pedido Finalizado com sucesso {0} - {1}", pedido.Id, pedido.PedidoStatus);
    }

    private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();

        var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepository.ObterPorId(message.PedidoId);
        pedido.CancelarPedido();

        pedidoRepository.Atualizar(pedido);

        if (!await pedidoRepository.UnityOfWork.Commit())
            throw new DomainException($"Problemas ao cancelar o pedido {message.PedidoId}");
    }

    private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
    {
        using var scope = _serviceProvider.CreateScope();

        var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        var pedido = await pedidoRepository.ObterPorId(message.PedidoId);

        pedido.FinalizarPedido();

        pedidoRepository.Atualizar(pedido);

        if (!await pedidoRepository.UnityOfWork.Commit())
            throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");
    }
}