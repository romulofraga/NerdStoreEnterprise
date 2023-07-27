﻿using Microsoft.Extensions.Options;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class CarrinhoService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoService(HttpClient httpClient, IOptions<AppSettings> setting)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(setting.Value.CarrinhoUrl);
        }

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho/");
            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemProdutoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PostAsync($"/carrinho/{produto.ProdutoId}", itemContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOK();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOK();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/{ProdutoId}");

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOK();
        }
    }
}
