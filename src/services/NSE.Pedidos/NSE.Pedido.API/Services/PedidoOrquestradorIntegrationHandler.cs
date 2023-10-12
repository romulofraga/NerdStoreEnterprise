using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.Queries;

namespace NSE.Pedidos.API.Services;

public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
{
    private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public PedidoOrquestradorIntegrationHandler(ILogger<PedidoOrquestradorIntegrationHandler> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedidos iniciado");

        _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Serviço de pedidos finalizado");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void ProcessarPedidos(object state)
    {
        using var scope = _serviceProvider.CreateScope();

        var pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();

        var pedido = await pedidoQueries.ObterPedidosAutorizados();

        if (pedido is null) return;

        var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

        var pedidoAutorizado = new PedidoAutorizadoIntegrationEvent(pedido.Id,
            pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

        #region GAMBIARRAGUID

        if (pedidoAutorizado.PedidoId == Guid.Empty)
        {
            _logger.LogCritical("PEDIDO SEM ID {PAID}", pedidoAutorizado.PedidoId);
            _logger.LogCritical("PEDIDO ORIGINAL ID {POID}", pedido.Id);
            return;
        }

        #endregion


        await bus.PublishAsync(pedidoAutorizado);

        _logger.LogInformation("Pedido {PedidoId} / {POID} foi encaminhado para baixa no estoque",
            pedidoAutorizado.PedidoId, pedido.Id);
    }
}