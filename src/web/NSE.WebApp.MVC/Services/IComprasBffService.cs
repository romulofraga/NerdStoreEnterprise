﻿using NSE.Core.Comunication;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public interface IComprasBffService
{
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(string voucher);
    Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
    Task<PedidoViewModel> ObterUltimoPedido();
    Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);

}