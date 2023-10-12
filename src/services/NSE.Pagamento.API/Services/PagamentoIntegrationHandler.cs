using NSE.Core.DomainObjects;
using NSE.MessageBus;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.API.Services;

namespace NSE.Core.Messages.Integration
{
    public class PagamentoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PagamentoIntegrationHandler> _logger;

        public PagamentoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<PagamentoIntegrationHandler> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _messageBus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
                await AutorizarPagamento(request));
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request =>
                await CancelarPagamento(request));

            _messageBus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque", async request =>
                await CapturarPagamento(request));

            #region GambiarraByPass

            _messageBus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque", request =>
            {
                _logger.LogDebug("INICIADO DEBUG _____======____");
                _logger.LogInformation("SIMULAÇÃO DE PEDIDO CAPTURADO - {PedidoId}", request.PedidoId);
                _logger.LogDebug("Finalizado DEBUG _____======____");
                return Task.CompletedTask;
            });

            #endregion
        }

        private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var pagamento = new Pagamento
            {
                PedidoId = message.PedidoId,
                TipoPagamento = (TipoPagamento)message.TipoPagamento,
                Valor = message.Valor,
                CartaoCredito = new CartaoCredito(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
            };

            var response = await pagamentoService.AutorizarPagamento(pagamento);

            return response;
        }

        private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CancelarPagamento(message.PedidoId);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
        }

        private async Task CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            
            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

            var response = await pagamentoService.CapturarPagamento(message.PedidoId);

            if (!response.ValidationResult.IsValid)
                throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

            await _messageBus.PublishAsync(new PedidoPagoIntegrationEvent(message.PedidoId));
        }
    }
}
