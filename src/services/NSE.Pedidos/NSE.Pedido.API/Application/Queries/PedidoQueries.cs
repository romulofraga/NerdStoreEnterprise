using Dapper;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterListaPorCliente(Guid clientId);
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
                                 AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                 AND P.PEDIDOSTATUS = 1 
                                 ORDER BY P.DATACADASTRO DESC";

            var pedido = await _pedidoRepository.ObterDBConnection()
                .QueryAsync<dynamic>(sql, new { clienteId });

            var lookup = new Dictionary<Guid, PedidoDTO>();


            var outrojeitoPedido = (await _pedidoRepository.ObterDBConnection()
                .QueryAsync<PedidoDTO, PedidoItemDTO, EnderecoDTO, PedidoDTO>(sql, (p, pi, e) =>
                {
                    if (!lookup.TryGetValue(p.Id, out var pedido))
                    {
                        lookup.Add(p.Id, pedido = p);
                        p.Endereco = e;
                    }

                    p.PedidoItems.Add(pi);

                    return p;
                }, new { clienteId })).FirstOrDefault();

            return MapearPedido(pedido);
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
