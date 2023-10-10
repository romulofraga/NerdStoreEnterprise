using Dapper;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterListaPorCliente(Guid clientId);

        Task<PedidoDTO> ObterPedidosAutorizados();
    }
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository, IServiceProvider serviceProvider)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorCliente(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            return pedidos.Select(PedidoDTO.ParaPedidoDTO);
        }

        public async Task<PedidoDTO> ObterUltimoPedido(Guid clienteId)
        {
            const string sql = @"SELECT
                                 P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL,P.PEDIDOSTATUS,
                                 P.LOGRADOURO,P.NUMERO, P.BAIRRO, P.CEP, P.COMPLEMENTO, P.CIDADE, P.ESTADO,
                                 PIT.ID AS 'ProdutoItemId',PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO 
                                 FROM PEDIDOS P 
                                 INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID 
                                 WHERE P.CLIENTEID = @clienteId 
                                 AND P.DATACADASTRO between DATEADD(minute, -5,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                 AND P.PEDIDOSTATUS = 1 
                                 ORDER BY P.DATACADASTRO DESC";

            var pedido = await _pedidoRepository.ObterDBConnection()
                .QueryAsync<dynamic>(sql, new { clienteId });

            return MapearPedido(pedido);
        }

        public async Task<PedidoDTO> ObterPedidosAutorizados()
        {
            const string sql = @"SELECT TOP 1
                                P.ID as 'PedidoId', P.ID, P.CLIENTEID,
                                PI.ID AS 'PedidoItemId', PI.ID, PI.PRODUTOID, PI.QUANTIDADE
                                FROM PEDIDOS P
                                INNER JOIN PEDIDOITEMS PI ON P.ID = PI.PEDIDOID
                                WHERE P.PEDIDOSTATUS = 1
                                ORDER BY P.DATACADASTRO";

            var pedido = await _pedidoRepository.ObterDBConnection().QueryAsync<PedidoDTO, PedidoItemDTO, PedidoDTO>(sql, (p, pi) =>
                {
                    p.PedidoItems = new List<PedidoItemDTO>();
                    p.PedidoItems.Add(pi);

                    return p;
                }, splitOn: "PedidoId,PedidoItemId");

            return pedido.FirstOrDefault();
        }

        private static PedidoDTO MapearPedido(dynamic resultado)
        {
            var pedido = new PedidoDTO
            {
                Codigo = resultado[0].CODIGO,
                Status = resultado[0].PEDIDOSTATUS,
                ValorTotal = resultado[0].VALORTOTAL,
                Desconto = resultado[0].DESCONTO,
                VoucherUtilizado = resultado[0].VOUCHERUTILIZADO,

                PedidoItems = new List<PedidoItemDTO>(),
                Endereco = new EnderecoDTO
                {
                    Logradouro = resultado[0].LOGRADOURO,
                    Bairro = resultado[0].BAIRRO,
                    Cep = resultado[0].CEP,
                    Cidade = resultado[0].CIDADE,
                    Complemento = resultado[0].COMPLEMENTO,
                    Estado = resultado[0].ESTADO,
                    Numero = resultado[0].NUMERO
                }
            };

            foreach (var item in resultado)
            {
                var pedidoItem = new PedidoItemDTO
                {
                    Nome = item.PRODUTONOME,
                    Valor = item.VALORUNITARIO,
                    Quantidade = item.QUANTIDADE,
                    Imagem = item.PRODUTOIMAGEM
                };

                pedido.PedidoItems.Add(pedidoItem);
            }

            return pedido;
        }
    }
}
