using NSE.Core.Data;
using System.Data.Common;

namespace NSE.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> ObterPorId(Guid pedidoId);
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorId(Guid itemId);
        Task<PedidoItem> ObterItemPorPedido(Guid PedidoId, Guid produtoId);

        DbConnection ObterDBConnection();
    }
}
