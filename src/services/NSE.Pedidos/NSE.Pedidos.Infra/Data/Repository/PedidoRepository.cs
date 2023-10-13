using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedidos.Domain.Pedidos;
using System.Data.Common;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosContext _context;
        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnityOfWork UnityOfWork => _context;

        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public async Task<PedidoItem> ObterItemPorId(Guid itemId)
        {
            return await _context.PedidoItems.FindAsync(itemId);
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid PedidoId, Guid produtoId)
        {
            return await _context.PedidoItems
                .FirstOrDefaultAsync(P => P.ProdutoId == produtoId && P.PedidoId == PedidoId);
        }

        public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItems)
                .AsNoTracking().Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<Pedido> ObterPorId(Guid pedidoId)
        {
            return await _context.Pedidos.FindAsync(pedidoId);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public DbConnection ObterDBConnection()
        {
            return _context.Database.GetDbConnection();
        }
    }
}
