using NSE.Pedidos.API.Application.DTO;

namespace NSE.Pedidos.API.Application.Queries;

public interface IPedidoQueries
{
    Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
    Task<IEnumerable<PedidoDTO>> ObterListaPorCliente(Guid clientId);

    Task<PedidoDTO> ObterPedidosAutorizados();
}