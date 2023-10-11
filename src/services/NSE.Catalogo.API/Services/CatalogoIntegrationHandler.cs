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

        public CatalogoIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
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
            _messageBus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request => await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
        {
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

            var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(message.ClienteId, message.PedidoId);
            await _messageBus.PublishAsync(pedidoBaixado);

        }

        private async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
            await _messageBus.PublishAsync(pedidoCancelado);
        }
    }
}