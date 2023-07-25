using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers;

public class CarrinhoController: MainController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICatalogoService _catalogoService;

    public CarrinhoController(ICarrinhoService carrinhoService, ICatalogoService catalogoService)
    {
        _carrinhoService = carrinhoService;
        _catalogoService = catalogoService;
    }

    [Route("carrinho")]
    public async Task<IActionResult> Index()
    {
        return View(await _carrinhoService.ObterCarrinho());
    }
    
    [HttpPost]
    [Route("carrinho/adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

        itemProduto.Nome = produto.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;
        
        ValidarItemCarrinho(produto, itemProduto.Quantidade);
        if (!OperacaoValida()) View("Index", await _carrinhoService.ObterCarrinho());

        var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);
        if (ResponsePossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Route("carrinho/atualizar-item")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
    { 
        var produto = await _catalogoService.ObterPorId(produtoId);
       
        ValidarItemCarrinho(produto, quantidade);
        if (!OperacaoValida()) View("Index", await _carrinhoService.ObterCarrinho());

        var itemProduto = new ItemProdutoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
        var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);
        if (ResponsePossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Route("carrinho/remover-item")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);
        if (produto == null)
        {
            AdicionarErrosValidacao("Produto inexistente");
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);
        if (ResponsePossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }
        
        return RedirectToAction("Index");
    }

    private void ValidarItemCarrinho(ProdutoViewModel produto, int quantidade)
    {
        if(produto == null) AdicionarErrosValidacao("Produto inexistente");
        if(quantidade < 1) AdicionarErrosValidacao($"Escolha ao menos uma unidade do produto {produto!.Nome}");
        if(quantidade > produto!.QuantidadeEstoque) AdicionarErrosValidacao($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque");
    }
}