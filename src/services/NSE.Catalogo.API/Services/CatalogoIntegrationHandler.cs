using NSE.Catalogo.API.Models;
using NSE.Core.DomainObjects;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Catalogo.API.Services
{
    public class CatalogoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CatalogoIntegrationHandler> _logger;

        public CatalogoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider, ILogger<CatalogoIntegrationHandler> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", 
                async request => await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            #region GAMBIARRABAIXARESTOQUE

            if (message.PedidoId == Guid.Empty)
            {
                _logger.LogCritical("CHEGOU NO CATALOGO SEM GUID - {Guid}", message.PedidoId);
                return;
            }

            #endregion

            using var scope = _serviceProvider.CreateScope();
            var produtosComEstoque = new List<Produto>();

            var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();
            var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
            var produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

            if (produtos.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtos)
            {
                var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;
                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add(produto);
                    _logger.LogInformation("{Quantidade} removidos do estoque do produto {ProdutoDescrição}", quantidadeProduto,
                        produto.Descricao);
                }
            }

            if (produtosComEstoque.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtosComEstoque)
            {
                produtoRepository.Atualizar(produto);
            }

            if (!await produtoRepository.UnityOfWork.Commit())
            {
                throw new DomainException($"Problema ao atualizar o estoque do pedido {message.PedidoId}");
            }

            var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(message.PedidoId);

            #region GAMBIARRAPEDIDOBAIXADO  

            if (pedidoBaixado.PedidoId == Guid.Empty)
            {
                _logger.LogCritical("CHEGOU NO CATALOGO SEM GUID");
                return;
            }

            #endregion
            
            await _messageBus.PublishAsync(pedidoBaixado);

        }

        private async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.PedidoId);
            await _messageBus.PublishAsync(pedidoCancelado);
        }
    }
}